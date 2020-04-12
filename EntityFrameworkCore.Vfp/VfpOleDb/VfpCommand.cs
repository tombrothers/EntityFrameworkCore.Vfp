using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using static System.String;

namespace EntityFrameworkCore.Vfp.VfpOleDb {
    #pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
    public class VfpCommand : VfpClient.VfpCommand, ICloneable {
        internal const string SplitCommandsToken = "{{{|VFP:EF:NEWCOMMAND|}}}";
        internal const string SingleRowTempTableName = "_VFP_EF_SINGLEROWTEMPTABLE";
        internal const string SingleRowTempTableRequiredToken = "{{{|VFP:EF:" + SingleRowTempTableName + "|}}}";
        internal const string ExecuteScalarBeginDelimiter = "{{{|VFP:EF:EXECUTESCALAR|";
        internal const string ExecuteScalarEndDelimiter = "|}}}";

        protected new internal OleDbCommand OleDbCommand { get { return base.OleDbCommand; } }

        public VfpCommand(string commandText = null, VfpClient.VfpConnection connection = null, VfpClient.VfpTransaction transaction = null) {

            CommandText = commandText;

            if(connection != null) {
                Connection = connection;
            }

            if(transaction != null) {
                Transaction = transaction;
            }
        }

        protected internal VfpCommand(VfpCommand vfpCommand)
            : base(vfpCommand) {
        }

        protected internal VfpCommand(OleDbCommand oleDbCommand, VfpClient.VfpConnection vfpConnection)
            : base(oleDbCommand, vfpConnection) {
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) {
            HandleTempTable();
            SplitMultipleCommands();
            HandleExecuteScalar();

            return (VfpClient.VfpDataReader)base.ExecuteDbDataReader(behavior);
        }

        public override int ExecuteNonQuery() {
            SplitMultipleCommands();

            return base.ExecuteNonQuery();
        }

        private void HandleTempTable() {
            if(!CommandText.Contains(SingleRowTempTableRequiredToken)) {
                return;
            }

            var commandText = CommandText;
            var tempFile = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(Connection.DataSource)), SingleRowTempTableName + ".dbf");

            if(!File.Exists(tempFile)) {
                CommandText = "CREATE TABLE '" + tempFile + "' FREE (PK i)";
                ExecuteNonQuery();
                CommandText = "INSERT INTO '" + tempFile + "' VALUE(1)";
                ExecuteNonQuery();
            }

            CommandText = commandText.Replace(SingleRowTempTableRequiredToken, "'" + tempFile + "'");
        }

        private void SplitMultipleCommands() {
            var commands = CommandText.Split(new[] { SplitCommandsToken }, StringSplitOptions.RemoveEmptyEntries).Where(x => !IsNullOrWhiteSpace(x)).ToArray();

            if(commands.Length <= 0) {
                return;
            }

            for(int index = 0, total = commands.Length - 1; index < total; index++) {
                CommandText = commands[index];
                HandleExecuteScalar();
                ExecuteNonQuery();
            }

            CommandText = commands[commands.Length - 1];
        }

        private void HandleExecuteScalar() {
            var startIndex = CommandText.IndexOf(ExecuteScalarBeginDelimiter);

            if(startIndex == -1) {
                return;
            }

            var endIndex = CommandText.IndexOf(ExecuteScalarEndDelimiter, startIndex);

            if(endIndex == -1) {
                return;
            }

            var commandText = CommandText;
            var fullText = CommandText.Substring(startIndex, (endIndex + ExecuteScalarEndDelimiter.Length) - startIndex);

            CommandText = fullText.Substring(ExecuteScalarBeginDelimiter.Length, fullText.Length - (ExecuteScalarEndDelimiter.Length + ExecuteScalarBeginDelimiter.Length));

            var value = ExecuteScalar();

            CommandText = commandText.Replace(fullText, GetFormattedValue(value));
        }

        private static string GetFormattedValue(object value) {
            if(value == null) {
                return " null ";
            }

            var stringValue = value as string;

            if(stringValue != null) {
                return "'" + stringValue.Replace("'", "' + chr(39) + '") + "'";
            }

            return value.ToString();
        }

        object ICloneable.Clone() {
            return new VfpCommand(this);
        }
    }
}

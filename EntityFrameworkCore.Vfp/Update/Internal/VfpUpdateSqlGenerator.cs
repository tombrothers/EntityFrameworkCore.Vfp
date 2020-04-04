using EntityFrameworkCore.Vfp.Update.Internal.Interfaces;
using EntityFrameworkCore.Vfp.VfpOleDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Update;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace EntityFrameworkCore.Vfp.Update.Internal {
    public class VfpUpdateSqlGenerator : UpdateSqlGenerator, IVfpUpdateSqlGenerator {
        public VfpUpdateSqlGenerator([NotNull] UpdateSqlGeneratorDependencies dependencies) : base(dependencies) {
        }

        public override ResultSetMapping AppendInsertOperation(
            StringBuilder commandStringBuilder,
            ModificationCommand command,
            int commandPosition
        ) {
            commandStringBuilder.ThrowIfNull(nameof(commandStringBuilder));
            command.ThrowIfNull(nameof(command));

            var name = command.TableName;
            var schema = command.Schema;
            var operations = command.ColumnModifications;
            var writeOperations = operations.Where(o => o.IsWrite).ToList();
            var readOperations = operations.Where(o => o.IsRead).ToList();

            AppendInsertCommand(commandStringBuilder, name, schema, writeOperations);

            if(readOperations.Count > 0) {
                var keyOperations = operations.Where(o => o.IsKey).ToList();

                return AppendSelectAffectedAfterInsertCommand(commandStringBuilder, name, schema, readOperations, keyOperations);
            }

            return ResultSetMapping.NoResultSet;
        }

        private ResultSetMapping AppendSelectAffectedAfterInsertCommand(
            [NotNull] StringBuilder commandStringBuilder,
            [NotNull] string name,
            [AllowNull] string schema,
            [NotNull] IReadOnlyList<ColumnModification> readOperations,
            [NotNull] IReadOnlyList<ColumnModification> conditionOperations
        ) {

            AppendSelectCommandHeader(commandStringBuilder, readOperations);
            AppendFromClause(commandStringBuilder, name, schema);
            AppendWhereAffectedClause(commandStringBuilder, conditionOperations);
            commandStringBuilder.AppendLine(SqlGenerationHelper.StatementTerminator)
                .AppendLine();

            return ResultSetMapping.LastInResultSet;
        }

        protected override void AppendWhereAffectedClause(
            [NotNull] StringBuilder commandStringBuilder,
            [NotNull] IReadOnlyList<ColumnModification> operations
        ) {
            commandStringBuilder.ThrowIfNull(nameof(commandStringBuilder));
            operations.ThrowIfNull(nameof(operations));

            commandStringBuilder
                .AppendLine()
                .Append("WHERE ");

            //AppendRowsAffectedWhereCondition(commandStringBuilder, 1);

            if(operations.Count > 0) {
                commandStringBuilder
                    //.Append(" AND ")
                    .AppendJoin(
                        operations, (sb, v) => {
                            if(v.IsKey) {
                                if(v.IsRead) {
                                    AppendIdentityWhereCondition(sb, v);
                                }
                                else {
                                    AppendWhereCondition(sb, v, v.UseOriginalValueParameter);
                                }
                            }
                        }, " AND ");
            }
        }

        protected override void AppendIdentityWhereCondition(
            [NotNull] StringBuilder commandStringBuilder,
            [NotNull] ColumnModification columnModification
        ) {
            commandStringBuilder
                .Append(columnModification.ColumnName)
                .Append("=")
                .Append(VfpCommand.ExecuteScalarBeginDelimiter)
                .Append("=")
                .Append(GetTableName(columnModification.Entry.EntityType))
                .Append(".")
                .Append(columnModification.ColumnName)
                .Append(VfpCommand.ExecuteScalarEndDelimiter)
                .Append(" ");
        }

        // Tried implementing this method using _Tally but it turns out that _tally isn't reliable.  
        protected override void AppendRowsAffectedWhereCondition([NotNull] StringBuilder commandStringBuilder, int expectedRowsAffected) { }

        private static string GetTableName(IEntityType entityType) {
            var tableNameAnnotation = entityType.GetAnnotation(RelationalAnnotationNames.TableName);

            return tableNameAnnotation.Value.ToString();
        }
    }
}

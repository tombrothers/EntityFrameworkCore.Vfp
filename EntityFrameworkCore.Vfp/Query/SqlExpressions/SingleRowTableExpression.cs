using EntityFrameworkCore.Vfp.VfpOleDb;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;

namespace EntityFrameworkCore.Vfp.Query.SqlExpressions {
    public class SingleRowTableExpression : TableExpressionBase {
        internal SingleRowTableExpression()
            : base("sr") {
            Name = VfpCommand.SingleRowTempTableRequiredToken;
        }

        protected override void Print(ExpressionPrinter expressionPrinter) {
            expressionPrinter.ThrowIfNull(nameof(expressionPrinter));

            expressionPrinter.Append(Name).Append(" AS ").Append(Alias);
        }

        public string Name { get; }

        public override bool Equals(object obj) => obj != null && ReferenceEquals(this, obj);

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Name);
    }
}

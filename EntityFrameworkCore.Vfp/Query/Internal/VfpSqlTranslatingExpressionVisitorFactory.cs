using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace EntityFrameworkCore.Vfp.Query.Internal {
    public class VfpSqlTranslatingExpressionVisitorFactory : IRelationalSqlTranslatingExpressionVisitorFactory {
        private readonly RelationalSqlTranslatingExpressionVisitorDependencies _dependencies;

        public VfpSqlTranslatingExpressionVisitorFactory(
            [NotNull] RelationalSqlTranslatingExpressionVisitorDependencies dependencies) {
            _dependencies = dependencies;
        }

        public RelationalSqlTranslatingExpressionVisitor Create(IModel model, QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor) =>
            new VfpSqlTranslatingExpressionVisitor(_dependencies, model, queryableMethodTranslatingExpressionVisitor);
    }
}

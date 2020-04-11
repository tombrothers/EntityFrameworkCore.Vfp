using EntityFrameworkCore.Vfp.Diagnostics.Internal;
using EntityFrameworkCore.Vfp.Infrastructure.Internal;
using EntityFrameworkCore.Vfp.Internal;
using EntityFrameworkCore.Vfp.Metadata.Conventions;
using EntityFrameworkCore.Vfp.Migrations;
using EntityFrameworkCore.Vfp.Migrations.Internal;
using EntityFrameworkCore.Vfp.Query;
using EntityFrameworkCore.Vfp.Query.Internal;
using EntityFrameworkCore.Vfp.Storage.Internal;
using EntityFrameworkCore.Vfp.Storage.Internal.Interfaces;
using EntityFrameworkCore.Vfp.Update.Internal;
using EntityFrameworkCore.Vfp.Update.Internal.Interfaces;
using EntityFrameworkCore.Vfp.ValueGeneration;
using EntityFrameworkCore.Vfp.ValueGeneration.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Extensions {
    public static class VfpServiceCollectionExtensions {
        public static IServiceCollection AddEntityFrameworkVfp([NotNull] this IServiceCollection serviceCollection) {
            serviceCollection.ThrowIfNull(nameof(serviceCollection));

            var builder = new EntityFrameworkRelationalServicesBuilder(serviceCollection)
                .TryAdd<LoggingDefinitions, VfpLoggingDefinitions>()
                .TryAdd<IDatabaseProvider, DatabaseProvider<VfpOptionsExtension>>()
                .TryAdd<IValueGeneratorCache>(p => p.GetService<IVfpValueGeneratorCache>())
                .TryAdd<IRelationalTypeMappingSource, VfpTypeMappingSource>()
                .TryAdd<ISqlGenerationHelper, VfpSqlGenerationHelper>()
                .TryAdd<IMigrationsAnnotationProvider, VfpMigrationsAnnotationProvider>()
                .TryAdd<IModelValidator, VfpModelValidator>()
                .TryAdd<IProviderConventionSetBuilder, VfpConventionSetBuilder>()
                .TryAdd<IUpdateSqlGenerator>(x => x.GetService<IVfpUpdateSqlGenerator>())
                .TryAdd<IModificationCommandBatchFactory, VfpModificationCommandBatchFactory>()
                .TryAdd<IValueGeneratorSelector, VfpValueGeneratorSelector>()
                .TryAdd<IRelationalConnection>(p => p.GetService<IVfpConnection>())
                .TryAdd<IMigrationsSqlGenerator, VfpMigrationsSqlGenerator>()
                .TryAdd<IRelationalDatabaseCreator, VfpDatabaseCreator>()
                .TryAdd<IHistoryRepository, VfpHistoryRepository>()
                .TryAdd<IExecutionStrategyFactory, VfpExecutionStrategyFactory>()
                .TryAdd<IRelationalQueryStringFactory, VfpQueryStringFactory>()
                .TryAdd<IQueryTranslationPostprocessorFactory, VfpQueryTranslationPostprocessorFactory>()
                // Query
                .TryAdd<IMethodCallTranslatorProvider, VfpMethodCallTranslatorProvider>()
                .TryAdd<IMemberTranslatorProvider, VfpMemberTranslatorProvider>()
                .TryAdd<IQuerySqlGeneratorFactory, VfpQuerySqlGeneratorFactory>()
                .TryAdd<IRelationalSqlTranslatingExpressionVisitorFactory, VfpSqlTranslatingExpressionVisitorFactory>()
                .TryAdd<IRelationalParameterBasedQueryTranslationPostprocessorFactory, VfpParameterBasedQueryTranslationPostprocessorFactory>()
                .TryAddProviderSpecificServices(x => x
                    .TryAddSingleton<IVfpValueGeneratorCache, VfpValueGeneratorCache>()
                    .TryAddSingleton<IVfpUpdateSqlGenerator, VfpUpdateSqlGenerator>()
                    .TryAddScoped<IVfpConnection, VfpConnection>()
                );

            ;

            builder.TryAddCoreServices();

            return serviceCollection;
        }
    }
}

using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExts {
    public static void AddDatabaseMigrations(this IServiceCollection services, string connectionString, Assembly migrationAssembly) {
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb =>
                rb.AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(migrationAssembly).For.Migrations()
        );

        services.AddScoped<DatabaseMigrationRunner, FluentMigratorMigrationRunner>();
    }
}
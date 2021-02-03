using System;
using System.Data;
using DetailingArsenal.Domain;
using DetailingArsenal.Persistence;
using DetailingArsenal.Tests.Persistence;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DetailingArsenal.Tests.Persistence {
    [TestClass]
    public static class DatabaseManager {
        public static IDatabase Database { get; private set; }

        [AssemblyInitialize]
        public static void Configure(TestContext context) {
            // https://stackoverflow.com/questions/60994309/net-core-3-1-loading-config-from-appsettings-json-for-console-application
            var c = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Tests.json")
                .Build();

            var dbConfig = new PostgresDatabaseConfig();
            c.Bind("Database", dbConfig);

            Database = new PostgresDatabase(dbConfig);

            var serviceProvider = new ServiceCollection();
            serviceProvider.AddDatabaseMigrations(dbConfig.GetConnectionString(), typeof(MigrationsFlag).Assembly);

            var services = serviceProvider.BuildServiceProvider();

            // Put the database update into a scope to ensure
            // that all resources will be disposed.
            using (var scope = services.CreateScope()) {
                // Instantiate the runner
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                // Execute the migrations
                runner.MigrateUp();
            }

        }
    }
}
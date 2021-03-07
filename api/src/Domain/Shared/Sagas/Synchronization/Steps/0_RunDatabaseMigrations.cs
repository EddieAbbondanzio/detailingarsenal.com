using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    [DependencyInjection]
    public class RunDatabaseMigrationsStep : SagaStep {
        IDatabaseMigrationRunner migrationRunner;

        public RunDatabaseMigrationsStep(IDatabaseMigrationRunner migrationRunner) {
            this.migrationRunner = migrationRunner;
        }

#pragma warning disable 1998
        public async override Task Execute(SagaContext context) {
            migrationRunner.MigrateUp();
        }
#pragma warning restore 1998
    }
}
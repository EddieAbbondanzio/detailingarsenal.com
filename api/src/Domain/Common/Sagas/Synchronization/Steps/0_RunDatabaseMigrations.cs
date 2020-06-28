using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Common {
    public class RunDatabaseMigrationsStep : SagaStep {
        IDatabaseMigrationRunner migrationRunner;

        public RunDatabaseMigrationsStep(IDatabaseMigrationRunner migrationRunner) {
            this.migrationRunner = migrationRunner;
        }

        public async override Task Execute() {
            migrationRunner.MigrateUp();
        }

        public async override Task Compensate() {
            throw new System.NotImplementedException();
        }
    }
}
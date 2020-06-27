using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Common {
    public class RunDatabaseMigrations : SagaStep {
        IDatabaseMigrationRunner migrationRunner;

        public RunDatabaseMigrations(IDatabaseMigrationRunner migrationRunner) {
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
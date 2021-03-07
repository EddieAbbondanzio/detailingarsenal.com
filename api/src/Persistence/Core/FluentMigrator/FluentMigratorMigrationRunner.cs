using DetailingArsenal.Domain;
using FluentMigrator.Runner;

namespace DetailingArsenal.Persistence {
    [DependencyInjection(RegisterAs = typeof(IDatabaseMigrationRunner), LifeTime = LifeTime.Scoped)]
    public sealed class FluentMigratorMigrationRunner : IDatabaseMigrationRunner {
        private IMigrationRunner runner;

        public FluentMigratorMigrationRunner(IMigrationRunner runner) {
            this.runner = runner;
        }

        public void MigrateDown(long version) {
            runner.MigrateDown(version);
        }

        public void MigrateUp() {
            runner.MigrateUp();
        }
    }
}
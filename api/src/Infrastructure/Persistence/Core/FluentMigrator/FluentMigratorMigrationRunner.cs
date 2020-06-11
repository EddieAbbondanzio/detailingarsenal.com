using DetailingArsenal.Domain;
using FluentMigrator.Runner;

namespace DetailingArsenal.Infrastructure {
    public sealed class FluentMigratorMigrationRunner : IDatabaseMigrationRunner {
        #region Fields
        private IMigrationRunner runner;
        #endregion

        #region Constructor(s)
        public FluentMigratorMigrationRunner(IMigrationRunner runner) {
            this.runner = runner;
        }

        public void MigrateDown(long version) {
            runner.MigrateDown(version);
        }

        public void MigrateUp() {
            runner.MigrateUp();
        }
        #endregion
    }
}
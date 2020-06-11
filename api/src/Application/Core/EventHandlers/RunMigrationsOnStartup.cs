using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class RunMigrationsOnStartup : IBusEventHandler<StartupEvent> {
        private IDatabaseMigrationRunner runner;

        public RunMigrationsOnStartup(IDatabaseMigrationRunner runner) {
            this.runner = runner;
        }

#pragma warning disable 1998
        public async Task Handle(StartupEvent busEvent) {
            runner.MigrateUp();
        }
#pragma warning restore 1998
    }
}
using System;
using System.Threading.Tasks;

public class RunMigrationsOnStartup : IBusEventHandler<StartupEvent> {
    private DatabaseMigrationRunner runner;

    public RunMigrationsOnStartup(DatabaseMigrationRunner runner) {
        this.runner = runner;
    }

#pragma warning disable 1998
    public async Task Handle(StartupEvent busEvent) {
        runner.MigrateUp();
    }
#pragma warning restore 1998
}
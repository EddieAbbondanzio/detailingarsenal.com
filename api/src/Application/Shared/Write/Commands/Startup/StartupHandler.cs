using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;
using Serilog;

namespace DetailingArsenal.Application {
    public class StartupHandler : ActionHandler<StartupCommand> {
        SynchronizationSaga saga;

        public StartupHandler(SynchronizationSaga saga) {
            this.saga = saga;
        }

        public async override Task Execute(StartupCommand input, User? user) {
            try {
                await saga.Execute();
            } catch (Exception e) {
                Log.Fatal($"Failed to execuite startup saga: {e.Message}");
                throw e;
            }
        }
    }
}
using System.Threading.Tasks;
using DetailingArsenal.Domain.Common;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Common {
    public class StartupHandler : ActionHandler<StartupCommand> {
        SynchronizationSaga saga;

        public StartupHandler(SynchronizationSaga saga) {
            this.saga = saga;
        }

        public async override Task Execute(StartupCommand input, User? user) {
            await saga.Execute();
        }
    }
}
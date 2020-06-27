using System.Threading.Tasks;
using DetailingArsenal.Domain.Common;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Common {
    public class SynchronizeHandler : ActionHandler<SynchronizeCommand> {
        SynchronizationSaga saga;

        public SynchronizeHandler(SynchronizationSaga saga) {
            this.saga = saga;
        }

        public async override Task Execute(SynchronizeCommand input, User? user) {
            await saga.Execute();
        }
    }
}
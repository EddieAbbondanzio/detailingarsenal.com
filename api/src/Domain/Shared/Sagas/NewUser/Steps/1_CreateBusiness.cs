using System.Threading.Tasks;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain {
    [DependencyInjection]
    public class CreateBusinessStep : SagaStep<string> {
        IBusinessService service;

        public CreateBusinessStep(IBusinessService service) {
            this.service = service;
        }

        public async override Task Execute(SagaContext<string> context) {
            await service.CreateDefault(context.Data.User);
        }

        public async override Task Compensate(SagaContext<string> context) {
            var b = await service.GetByUser(context.Data.User);
            await service.Delete(b);
        }
    }
}
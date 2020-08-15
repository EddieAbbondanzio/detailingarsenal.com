
using System.Threading.Tasks;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Common {
    public class CreateHoursOfOperationStep : SagaStep<string> {
        IHoursOfOperationService service;

        public CreateHoursOfOperationStep(IHoursOfOperationService service) {
            this.service = service;
        }

        public async override Task Execute(SagaContext<string> context) {
            await service.CreateDefault(context.Data.User);
        }

        public async override Task Compensate(SagaContext<string> context) {
            var hours = await service.GetByUser(context.Data.User);
            await service.Delete(hours);
        }
    }
}
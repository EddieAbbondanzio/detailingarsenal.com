
using System.Threading.Tasks;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Common {
    public class CreateHoursOfOperationStep : SagaStep<User> {
        IHoursOfOperationService service;

        public CreateHoursOfOperationStep(IHoursOfOperationService service) {
            this.service = service;
        }

        public async override Task Execute(User user) {
            await service.CreateDefault(user);
        }

        public async override Task Compensate(User user) {
            var hours = await service.GetByUser(user);
            await service.Delete(hours);
        }
    }
}
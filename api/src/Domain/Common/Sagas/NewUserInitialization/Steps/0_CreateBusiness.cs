using System.Threading.Tasks;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Common {
    public class CreateBusinessStep : SagaStep<User> {
        IBusinessService service;

        public CreateBusinessStep(IBusinessService service) {
            this.service = service;
        }

        public async override Task Execute(User user) {
            await service.CreateDefault(user);
        }

        public async override Task Compensate(User user) {
            var b = await service.GetByUser(user);
            await service.Delete(b);
        }
    }
}
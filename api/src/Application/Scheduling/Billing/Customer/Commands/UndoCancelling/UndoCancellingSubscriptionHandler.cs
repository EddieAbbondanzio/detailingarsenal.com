using System.Threading.Tasks;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    public class UndoCancellingSubscriptionHandler : ActionHandler<UndoCancellingSubscriptionCommand> {
        ICustomerService service;

        public UndoCancellingSubscriptionHandler(ICustomerService service) {
            this.service = service;
        }

        public async override Task Execute(UndoCancellingSubscriptionCommand input, User? user) {
            var c = await service.GetByUser(user!);
            await service.UndoCancellingSubscription(c);
        }
    }
}
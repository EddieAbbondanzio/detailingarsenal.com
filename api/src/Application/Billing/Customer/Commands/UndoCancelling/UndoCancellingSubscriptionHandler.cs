using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Billing {
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
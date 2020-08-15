using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Billing {
    public class CancelSubscriptionAtPeriodEndHandler : ActionHandler<CancelSubscriptionAtPeriodEndCommand> {
        ICustomerService service;

        public CancelSubscriptionAtPeriodEndHandler(ICustomerService service) {
            this.service = service;
        }

        public async override Task Execute(CancelSubscriptionAtPeriodEndCommand input, User? user) {
            var c = await service.GetByUser(user!);
            await service.CancelSubscriptionAtPeriodEnd(c);
        }
    }
}
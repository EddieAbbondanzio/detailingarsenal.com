using System.Threading.Tasks;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
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
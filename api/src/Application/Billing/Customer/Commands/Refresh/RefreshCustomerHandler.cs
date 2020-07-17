using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Billing {
    /// <summary>
    /// Refresh the local copy of the customer against the third party billing resource.
    /// </summary>
    public class RefreshCustomerHandler : ActionHandler<RefreshCustomerCommand> {
        ICustomerService customerService;

        public RefreshCustomerHandler(ICustomerService customerService) {
            this.customerService = customerService;
        }

        public async override Task Execute(RefreshCustomerCommand input, User? user) {
            // User will be null due to this being called via a webhook.

            var c = await customerService.GetByBillingId(input.BillingId);
            await customerService.Refresh(c);
        }
    }
}
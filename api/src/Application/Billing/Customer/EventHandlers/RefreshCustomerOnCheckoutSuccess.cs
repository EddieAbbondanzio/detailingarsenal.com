using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;

namespace DetailingArsenal.Application.Billing {
    public class RefreshCustomerOnCheckoutSuccess : IDomainEventSubscriber<CheckoutSessionCompletedSuccessfully> {
        ICustomerService customerService;

        public RefreshCustomerOnCheckoutSuccess(ICustomerService customerService) {
            this.customerService = customerService;
        }

        public async Task Notify(CheckoutSessionCompletedSuccessfully busEvent) {
            var c = await customerService.GetByBillingId(busEvent.CustomerBillingId);
            await customerService.Refresh(c);
        }
    }
}
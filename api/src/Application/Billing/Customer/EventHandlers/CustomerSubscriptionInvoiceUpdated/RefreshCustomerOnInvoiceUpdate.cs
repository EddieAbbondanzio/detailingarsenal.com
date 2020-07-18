using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;

namespace DetailingArsenal.Application.Billing {
    public class RefreshCustomerOnInvoiceUpdate : IDomainEventSubscriber<CustomerSubscriptionInvoiceUpdated> {
        ICustomerService customerService;

        public RefreshCustomerOnInvoiceUpdate(ICustomerService customerService) {
            this.customerService = customerService;
        }

        public async Task Notify(CustomerSubscriptionInvoiceUpdated busEvent) {
            var customer = await customerService.GetByBillingId(busEvent.CustomerBillingId);

            await customerService.Refresh(customer);
        }
    }
}
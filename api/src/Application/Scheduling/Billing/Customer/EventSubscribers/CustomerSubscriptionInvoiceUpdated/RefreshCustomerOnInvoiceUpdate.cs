using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;

namespace DetailingArsenal.Application.Scheduling.Billing {
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
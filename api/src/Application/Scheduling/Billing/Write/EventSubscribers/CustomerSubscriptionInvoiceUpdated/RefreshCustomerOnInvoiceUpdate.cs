using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;

namespace DetailingArsenal.Application.Scheduling.Billing {
    [DependencyInjection(RegisterAs = typeof(IDomainEventSubscriber<CustomerSubscriptionInvoiceUpdated>))]
    public class RefreshCustomerOnInvoiceUpdate : IDomainEventSubscriber<CustomerSubscriptionInvoiceUpdated> {
        ICustomerRepo repo;
        ICustomerRefresher customerService;

        public RefreshCustomerOnInvoiceUpdate(ICustomerRepo repo, ICustomerRefresher customerService) {
            this.repo = repo;
            this.customerService = customerService;
        }

        public async Task Notify(CustomerSubscriptionInvoiceUpdated busEvent) {
            var customer = await repo.FindByBillingId(busEvent.CustomerBillingId) ?? throw new EntityNotFoundException();

            await customerService.Refresh(customer);
        }
    }
}
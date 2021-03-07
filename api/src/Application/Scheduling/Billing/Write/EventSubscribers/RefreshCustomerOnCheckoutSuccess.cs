using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;

namespace DetailingArsenal.Application.Scheduling.Billing {
    [DependencyInjection(RegisterAs = typeof(IDomainEventSubscriber<CheckoutSessionCompletedSuccessfully>))]
    public class RefreshCustomerOnCheckoutSuccess : IDomainEventSubscriber<CheckoutSessionCompletedSuccessfully> {
        ICustomerRepo repo;
        ICustomerRefresher customerService;

        public RefreshCustomerOnCheckoutSuccess(ICustomerRepo repo, ICustomerRefresher customerService) {
            this.repo = repo;
            this.customerService = customerService;
        }

        public async Task Notify(CheckoutSessionCompletedSuccessfully busEvent) {
            var c = await repo.FindByBillingId(busEvent.CustomerBillingId) ?? throw new EntityNotFoundException();
            await customerService.Refresh(c);

            // Delete out old payment methods besides latest one (on our side).
            c.PaymentMethods = c.PaymentMethods.Where(pm => pm.BillingReference.BillingId == busEvent.NewPaymentMethodBillingId).ToList();
            await repo.Update(c);
        }
    }
}
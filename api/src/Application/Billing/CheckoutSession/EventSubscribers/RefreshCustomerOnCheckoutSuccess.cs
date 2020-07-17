using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;

namespace DetailingArsenal.Application.Billing {
    public class RefreshCustomerOnCheckoutSuccess : IDomainEventSubscriber<CheckoutSessionCompletedSuccessfully> {
        public Task Notify(CheckoutSessionCompletedSuccessfully busEvent) {
            throw new System.NotImplementedException();
        }
    }
}
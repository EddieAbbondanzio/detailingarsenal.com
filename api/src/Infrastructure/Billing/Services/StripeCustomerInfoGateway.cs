
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using Stripe;

namespace DetailingArsenal.Infrastructure {
    public class StripeCustomerInfoGateway : ICustomerInfoGateway {
        Stripe.CustomerService customerService;

        public StripeCustomerInfoGateway() {
            this.customerService = new CustomerService();
        }

        public async Task<CustomerInfo> Create(string email) {
            var c = await customerService.CreateAsync(new CustomerCreateOptions {
                Email = email
            });

            return Map(c);
        }

        public async Task<CustomerInfo> FindByExternalId(string externalId) {
            var c = await customerService.GetAsync(externalId);
            return Map(c);
        }

        CustomerInfo Map(Stripe.Customer customer) {
            return new CustomerInfo(customer.Id, customer.Email);
        }
    }
}
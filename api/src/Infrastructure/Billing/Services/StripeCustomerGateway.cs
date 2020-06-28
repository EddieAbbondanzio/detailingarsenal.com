
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using Stripe;

namespace DetailingArsenal.Infrastructure {
    public class StripeCustomerGateway : ICustomerGateway {
        Stripe.CustomerService customerService;

        public StripeCustomerGateway() {
            this.customerService = new CustomerService();
        }

        public async Task<ExternalCustomer> Create(string email) {
            var c = await customerService.CreateAsync(new CustomerCreateOptions {
                Email = email
            });

            return Map(c);
        }

        public async Task<ExternalCustomer> FindByExternalId(string externalId) {
            var c = await customerService.GetAsync(externalId);
            return Map(c);
        }

        ExternalCustomer Map(Stripe.Customer customer) {
            return new ExternalCustomer(customer.Id, customer.Email);
        }
    }
}
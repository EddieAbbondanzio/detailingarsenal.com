
using System.Threading.Tasks;
using Stripe;

public class StripeCustomerInfoService : ICustomerInfoService {
    Stripe.CustomerService customerService;

    public StripeCustomerInfoService() {
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
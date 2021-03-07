using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    public interface ICustomerRefresher : IService {
        Task Refresh(Customer customer);    // Don't delete
    }

    [DependencyInjection(RegisterAs = typeof(ICustomerRefresher))]
    public class CustomerRefresher : ICustomerRefresher {
        ICustomerGateway customerGateway;
        ICustomerRepo customerRepo;

        public CustomerRefresher(ICustomerGateway customerGateway, ICustomerRepo customerRepo) {
            this.customerGateway = customerGateway;
            this.customerRepo = customerRepo;
        }

        public async Task Refresh(Customer customer) {
            var refreshedCustomer = await customerGateway.GetByBillingId(customer.BillingReference.BillingId);

            customer.Subscription = refreshedCustomer.Subscription;
            customer.PaymentMethods = refreshedCustomer.PaymentMethods;

            await customerRepo.Update(customer);
        }
    }
}
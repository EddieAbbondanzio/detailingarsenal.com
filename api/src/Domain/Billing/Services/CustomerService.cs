using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Billing {
    public interface ICustomerService : IService {
        Task<Customer> CreateTrialCustomer(User user, SubscriptionPlan plan);
        Task DeleteForUser(User user);
    }

    public class CustomerService : ICustomerService {
        ICustomerGateway customerGateway;
        ICustomerRepo customerRepo;

        public CustomerService(ICustomerGateway customerGateway, ICustomerRepo customerRepo) {
            this.customerGateway = customerGateway;
            this.customerRepo = customerRepo;
        }

        public async Task<Customer> CreateTrialCustomer(User user, SubscriptionPlan plan) {
            var customer = await customerGateway.CreateTrialCustomer(user, plan);
            await customerRepo.Add(customer);

            return customer;
        }

        public async Task DeleteForUser(User user) {
            var customer = await customerRepo.FindByUser(user);

            if (customer == null) {
                return;
            }

            await customerGateway.Delete(customer);
            await customerRepo.Delete(customer);
        }
    }
}
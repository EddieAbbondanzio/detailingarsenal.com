using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    public interface ICustomerService : IService {
        Task<Customer> GetByUser(User user);
        Task<Customer> GetByBillingId(string billingId);
        Task<Customer> StartSubscription(User user, SubscriptionPlan plan);
        Task CancelSubscriptionAtPeriodEnd(Customer customer);
        Task UndoCancellingSubscription(Customer customer);
        Task DeleteForUser(User user);
        Task Refresh(Customer customer);
        Task Update(Customer customer);
    }

    public class CustomerService : ICustomerService {
        ICustomerGateway customerGateway;
        ICustomerRepo customerRepo;

        public CustomerService(ICustomerGateway customerGateway, ICustomerRepo customerRepo) {
            this.customerGateway = customerGateway;
            this.customerRepo = customerRepo;
        }

        public async Task<Customer> GetByUser(User user) {
            var customer = await customerRepo.FindByUser(user);
            return customer ?? throw new EntityNotFoundException();
        }

        public async Task<Customer> GetByBillingId(string billingId) {
            return await customerRepo.FindByBillingId(billingId) ?? throw new EntityNotFoundException();
        }

        public async Task<Customer> StartSubscription(User user, SubscriptionPlan plan) {
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

        public async Task Refresh(Customer customer) {
            var refreshedCustomer = await customerGateway.GetByBillingId(customer.BillingReference.BillingId);

            customer.Subscription = refreshedCustomer.Subscription;
            customer.PaymentMethods = refreshedCustomer.PaymentMethods;

            await customerRepo.Update(customer);
        }

        public async Task CancelSubscriptionAtPeriodEnd(Customer customer) {
            await customerGateway.CancelSubscriptionAtPeriodEnd(customer);
            await customerRepo.Update(customer);
        }

        public async Task UndoCancellingSubscription(Customer customer) {
            await customerGateway.UndoCancellingSubscription(customer);
            await customerRepo.Update(customer);
        }

        public async Task Update(Customer customer) {
            await customerRepo.Update(customer);
        }
    }
}
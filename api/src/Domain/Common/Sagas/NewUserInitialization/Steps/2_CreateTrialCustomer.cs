using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Common {
    public class CreateCustomerAndStartSubscriptionStep : SagaStep<User> {
        ICustomerService customerService;

        public CreateCustomerAndStartSubscriptionStep(ICustomerService customerService) {
            this.customerService = customerService;
        }

        public override async Task Execute(User user) {
            await customerService.CreateTrialCustomer(user);
        }

        public async override Task Compensate(User user) {
            throw new NotImplementedException();
        }
    }
}
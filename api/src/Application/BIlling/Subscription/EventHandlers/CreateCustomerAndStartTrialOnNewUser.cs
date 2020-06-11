using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using Serilog;

namespace DetailingArsenal.Application {
    public class CreateCustomerAndStartTrialOnNewUser : IBusEventHandler<NewUserEvent> {
        private ICustomerRepo customerRepo;
        private ISubscriptionRepo subscriptionRepo;
        private ISubscriptionService subscriptionService;

        public CreateCustomerAndStartTrialOnNewUser(ICustomerRepo customerRepo, ISubscriptionRepo subscriptionRepo, ISubscriptionService subscriptionService) {
            this.customerRepo = customerRepo;
            this.subscriptionRepo = subscriptionRepo;
            this.subscriptionService = subscriptionService;
        }

        public async Task Handle(NewUserEvent busEvent) {
            // Create the customer
            var customer = new Customer() {
                Id = Guid.NewGuid(),
                UserId = busEvent.User.Id,
                Info = new CustomerInfo(null!, busEvent.User.Email)
            };

            await customerRepo.Add(customer);

            // Now create and start the trial subscription
            var sub = await subscriptionService.CreateTrialSubscription(customer);
            await subscriptionRepo.Add(sub);
        }
    }
}
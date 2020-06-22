using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using Serilog;

namespace DetailingArsenal.Application {
    public class CreateCustomerAndStartTrialOnNewUser : IBusEventHandler<NewUserEvent> {
        private ICustomerRepo customerRepo;
        private ISubscriptionRepo subscriptionRepo;
        private IRoleRepo roleRepo;
        private IExternalSubscriptionGateway subscriptionGateway;

        public CreateCustomerAndStartTrialOnNewUser(ICustomerRepo customerRepo, ISubscriptionRepo subscriptionRepo, IRoleRepo roleRepo, IExternalSubscriptionGateway subscriptionService) {
            this.customerRepo = customerRepo;
            this.subscriptionRepo = subscriptionRepo;
            this.roleRepo = roleRepo;
            this.subscriptionGateway = subscriptionService;
        }

        public async Task Handle(NewUserEvent busEvent) {
            // Create the customer
            var customer = Customer.Create(
                busEvent.User.Id,
                new ExternalCustomer(null!, busEvent.User.Email)
            );

            await customerRepo.Add(customer);

            // Now create and start the trial subscription
            // var sub = await subscriptionGateway.CreateTrialSubscription(customer);
            // await subscriptionRepo.Add(sub);

            throw new Exception();

            // get the plan

            // create their role
        }
    }
}
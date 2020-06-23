using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using Serilog;

namespace DetailingArsenal.Application {
    /// <summary>
    /// Jesus christ refactor this.
    /// </summary>
    public class CreateCustomerAndStartTrialOnNewUser : IBusEventHandler<NewUserEvent> {
        ISubscriptionConfig config;
        private ICustomerRepo customerRepo;
        private ISubscriptionRepo subscriptionRepo;
        private ISubscriptionPlanRepo planRepo;
        private IRoleRepo roleRepo;
        private IExternalSubscriptionGateway subscriptionGateway;

        public CreateCustomerAndStartTrialOnNewUser(ISubscriptionConfig config, ICustomerRepo customerRepo, ISubscriptionRepo subscriptionRepo, ISubscriptionPlanRepo planRepo, IRoleRepo roleRepo, IExternalSubscriptionGateway subscriptionService) {
            this.config = config;
            this.customerRepo = customerRepo;
            this.subscriptionRepo = subscriptionRepo;
            this.planRepo = planRepo;
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
            var plan = await planRepo.FindByExternalId(config.DefaultPlan) ?? throw new Exception("No default subscription plan specified");

            var sub = await subscriptionGateway.CreateTrialSubscription(plan, customer);
            await subscriptionRepo.Add(sub);
        }
    }
}
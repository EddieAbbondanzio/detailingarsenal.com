using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Common {
    public class CreateTrialCustomerStep : SagaStep<User> {
        ICustomerService customerService;
        ISubscriptionPlanService subscriptionPlanService;

        public CreateTrialCustomerStep(ICustomerService customerService, ISubscriptionPlanService subscriptionPlanService) {
            this.customerService = customerService;
            this.subscriptionPlanService = subscriptionPlanService;
        }

        public override async Task Execute(User user) {
            var trialPlan = await subscriptionPlanService.GetTrialPlan();
            await customerService.CreateTrialCustomer(user, trialPlan);
        }

        public async override Task Compensate(User user) {
            await customerService.DeleteForUser(user);
        }
    }
}
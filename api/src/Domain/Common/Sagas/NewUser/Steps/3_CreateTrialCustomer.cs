using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Common {
    public class CreateTrialCustomerStep : SagaStep<string> {
        ICustomerService customerService;
        ISubscriptionPlanService subscriptionPlanService;

        public CreateTrialCustomerStep(ICustomerService customerService, ISubscriptionPlanService subscriptionPlanService) {
            this.customerService = customerService;
            this.subscriptionPlanService = subscriptionPlanService;
        }

        public override async Task Execute(SagaContext<string> context) {
            var trialPlan = await subscriptionPlanService.GetTrialPlan();
            await customerService.CreateTrialCustomer(context.Data.User, trialPlan);
        }

        public async override Task Compensate(SagaContext<string> context) {
            await customerService.DeleteForUser(context.Data.User);
        }
    }
}
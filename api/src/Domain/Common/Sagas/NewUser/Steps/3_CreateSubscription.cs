using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Common {
    public class CreateSubscriptionStep : SagaStep<string> {
        ICustomerService customerService;
        ISubscriptionPlanRepo planRepo;

        public CreateSubscriptionStep(ICustomerService customerService, ISubscriptionPlanRepo planRepo) {
            this.customerService = customerService;
            this.planRepo = planRepo;
        }

        public override async Task Execute(SagaContext<string> context) {
            var trialPlan = await planRepo.FindDefault();
            await customerService.StartSubscription(context.Data.User, trialPlan);

            context.Data.Plan = trialPlan;
        }

        public async override Task Compensate(SagaContext<string> context) {
            await customerService.DeleteForUser(context.Data.User);
        }
    }
}
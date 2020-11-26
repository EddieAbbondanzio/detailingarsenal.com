using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Common {
    public class CreateSubscriptionStep : SagaStep<string> {
        ICustomerGateway customerGateway;
        ICustomerRepo customerRepo;
        ISubscriptionPlanRepo planRepo;

        public CreateSubscriptionStep(ICustomerGateway customerGateway, ICustomerRepo customerRepo, ISubscriptionPlanRepo planRepo) {
            this.customerGateway = customerGateway;
            this.customerRepo = customerRepo;
            this.planRepo = planRepo;
        }

        public override async Task Execute(SagaContext<string> context) {
            var trialPlan = await planRepo.FindDefault();
            var customer = await customerGateway.CreateTrialCustomer(context.Data.User, trialPlan);
            await customerRepo.Add(customer);

            context.Data.Plan = trialPlan;
        }

        public async override Task Compensate(SagaContext<string> context) {
            var customer = await customerRepo.FindByUser(context.Data.User);

            if (customer == null) {
                return;
            }

            await customerGateway.Delete(customer);
            await customerRepo.Delete(customer);
        }
    }
}
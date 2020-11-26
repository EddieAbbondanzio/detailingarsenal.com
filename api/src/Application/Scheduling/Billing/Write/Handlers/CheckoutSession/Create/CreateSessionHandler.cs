using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    [Authorization]
    public class CreateSessionHandler : ActionHandler<CreateCheckoutSessionCommand, BillingReference> {
        ICustomerRepo customerRepo;
        ICheckoutSessionGateway sessionGateway;

        public CreateSessionHandler(ICustomerRepo customerRepo, ICheckoutSessionGateway sessionGateway) {
            this.customerRepo = customerRepo;
            this.sessionGateway = sessionGateway;
        }

        public async override Task<BillingReference> Execute(CreateCheckoutSessionCommand input, User? user) {
            var customer = await customerRepo.FindByUser(user!) ?? throw new EntityNotFoundException();
            var billingReference = await sessionGateway.CreateSession(customer, input.PriceBillingId);
            return billingReference;
        }
    }
}
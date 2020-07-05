using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Billing {
    [Authorization]
    public class CreateSessionHandler : ActionHandler<CreateSessionCommand, BillingReference> {
        ICustomerService customerService;
        ISessionGateway sessionGateway;

        public CreateSessionHandler(ICustomerService customerService, ISessionGateway sessionGateway) {
            this.customerService = customerService;
            this.sessionGateway = sessionGateway;
        }

        public async override Task<BillingReference> Execute(CreateSessionCommand input, User? user) {
            var customer = await customerService.GetByUser(user!);
            var billingReference = await sessionGateway.CreateSession(customer, input.PriceBillingId);
            return billingReference;
        }
    }
}
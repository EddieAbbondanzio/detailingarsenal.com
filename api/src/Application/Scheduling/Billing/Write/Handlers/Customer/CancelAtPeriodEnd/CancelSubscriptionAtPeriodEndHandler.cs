using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    public class CancelSubscriptionAtPeriodEndHandler : ActionHandler<CancelSubscriptionAtPeriodEndCommand> {
        ICustomerGateway customerGateway;
        ICustomerRepo customerRepo;

        public CancelSubscriptionAtPeriodEndHandler(ICustomerGateway customerGateway, ICustomerRepo customerRepo) {
            this.customerGateway = customerGateway;
            this.customerRepo = customerRepo;
        }

        public async override Task Execute(CancelSubscriptionAtPeriodEndCommand input, User? user) {
            var customer = await customerRepo.FindByUser(user!) ?? throw new EntityNotFoundException();
            await customerGateway.CancelSubscriptionAtPeriodEnd(customer);
            await customerRepo.Update(customer);
        }
    }
}
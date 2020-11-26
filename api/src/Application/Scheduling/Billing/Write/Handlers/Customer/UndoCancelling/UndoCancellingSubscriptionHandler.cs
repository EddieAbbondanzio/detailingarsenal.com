using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    public class UndoCancellingSubscriptionHandler : ActionHandler<UndoCancellingSubscriptionCommand> {
        ICustomerGateway customerGateway;
        ICustomerRepo customerRepo;

        public UndoCancellingSubscriptionHandler(ICustomerGateway customerGateway, ICustomerRepo customerRepo) {
            this.customerGateway = customerGateway;
            this.customerRepo = customerRepo;
        }

        public async override Task Execute(UndoCancellingSubscriptionCommand input, User? user) {
            var customer = await customerRepo.FindByUser(user!) ?? throw new EntityNotFoundException();
            await customerGateway.UndoCancellingSubscription(customer);
            await customerRepo.Update(customer);
        }
    }
}
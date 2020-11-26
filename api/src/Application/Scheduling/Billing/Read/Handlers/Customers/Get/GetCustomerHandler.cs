using System.Threading.Tasks;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    public class GetCustomerHandler : ActionHandler<GetCustomerQuery, CustomerReadModel> {
        ICustomerReader customerReader;

        public GetCustomerHandler(ICustomerReader customerReader) {
            this.customerReader = customerReader;
        }

        public async override Task<CustomerReadModel> Execute(GetCustomerQuery input, User? user) {
            var c = await customerReader.ReadForUser(user!);
            return c;
        }
    }
}
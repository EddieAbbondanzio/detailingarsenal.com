using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    [DependencyInjection]
    public class GetByIdSubscriptionPlanHandler : ActionHandler<GetByIdSubscriptionPlanQuery, SubscriptionPlanReadModel?> {
        ISubscriptionPlanReader reader;

        public GetByIdSubscriptionPlanHandler(ISubscriptionPlanReader reader) {
            this.reader = reader;
        }

        public async override Task<SubscriptionPlanReadModel?> Execute(GetByIdSubscriptionPlanQuery input, User? user) {
            var p = await reader.ReadById(input.Id);
            return p;
        }
    }
}
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    [DependencyInjection]
    public class GetDefaultSubscriptionPlanHandler : ActionHandler<GetDefaultSubscriptionPlanQuery, SubscriptionPlanReadModel> {
        ISubscriptionPlanReader reader;

        public GetDefaultSubscriptionPlanHandler(ISubscriptionPlanReader reader) {
            this.reader = reader;
        }

        public async override Task<SubscriptionPlanReadModel> Execute(GetDefaultSubscriptionPlanQuery input, User? user) {
            var p = await reader.ReadDefault();
            return p;
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    [Authorization(Action = "read", Scope = "subscription-plans")]
    [DependencyInjection]
    public class GetAllSubscriptionPlansHandler : ActionHandler<GetAllSubscriptionPlansQuery, List<SubscriptionPlanReadModel>> {
        ISubscriptionPlanReader reader;

        public GetAllSubscriptionPlansHandler(ISubscriptionPlanReader reader) {
            this.reader = reader;
        }

        public async override Task<List<SubscriptionPlanReadModel>> Execute(GetAllSubscriptionPlansQuery input, User? user) {
            return await reader.ReadAll();
        }
    }
}
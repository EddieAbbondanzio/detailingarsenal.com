using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    [Authorization(Action = "read", Scope = "subscription-plans")]
    public class GetSubscriptionPlansHandler : ActionHandler<GetSubscriptionPlansQuery, List<SubscriptionPlanReadModel>> {
        ISubscriptionPlanReader reader;

        public GetSubscriptionPlansHandler(ISubscriptionPlanReader reader) {
            this.reader = reader;
        }

        public async override Task<List<SubscriptionPlanReadModel>> Execute(GetSubscriptionPlansQuery input, User? user) {
            return await reader.ReadAll();
        }
    }
}
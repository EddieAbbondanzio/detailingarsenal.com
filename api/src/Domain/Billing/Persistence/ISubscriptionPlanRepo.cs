using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface ISubscriptionPlanRepo : IRepo<SubscriptionPlan> {
        Task<List<SubscriptionPlan>> FindAll();
        Task<SubscriptionPlan?> FindByExternalId(string externalId);
    }
}
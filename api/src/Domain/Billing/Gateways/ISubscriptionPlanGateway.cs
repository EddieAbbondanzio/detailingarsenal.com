using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Billing {
    public interface ISubscriptionPlanGateway : IGateway {
        Task<SubscriptionPlan> GetByExternalId(string externalId);
        Task<List<SubscriptionPlan>> GetAll();
        Task<List<SubscriptionPlan>> RefreshPlans();
    }
}
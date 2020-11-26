using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    public interface ISubscriptionPlanGateway : IGateway {
        Task<List<SubscriptionPlan>> GetAll();
    }
}
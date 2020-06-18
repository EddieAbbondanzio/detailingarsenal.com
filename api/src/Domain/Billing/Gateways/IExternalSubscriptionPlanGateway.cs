using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface IExternalSubscriptionPlanGateway : IService {
        Task<List<ExternalSubscriptionPlan>> GetAll();
    }
}
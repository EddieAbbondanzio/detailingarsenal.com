using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface ISubscriptionPlanInfoGateway : IService {
        Task<List<SubscriptionPlanInfo>> GetAll();
    }
}
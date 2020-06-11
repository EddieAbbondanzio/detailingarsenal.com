using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface ISubscriptionPlanInfoService : IService {
        Task<List<SubscriptionPlanInfo>> GetAll();
    }
}
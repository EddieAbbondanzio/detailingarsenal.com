using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface IPlanGateway : IGateway {
        Task<List<ExternalSubscriptionPlan>> GetAll();
    }
}
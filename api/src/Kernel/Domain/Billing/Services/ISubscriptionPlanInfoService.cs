using System.Collections.Generic;
using System.Threading.Tasks;

public interface ISubscriptionPlanInfoService : IService {
    Task<List<SubscriptionPlanInfo>> GetAll();
}
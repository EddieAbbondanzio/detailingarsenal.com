using System.Collections.Generic;
using System.Threading.Tasks;

public interface ISubscriptionPlanRepo : IRepo<SubscriptionPlan> {
    Task<List<SubscriptionPlan>> FindAll();
    Task<SubscriptionPlan?> FindByName(string name);
}
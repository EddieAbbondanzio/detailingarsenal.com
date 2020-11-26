using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    public interface ISubscriptionPlanRepo : IRepo<SubscriptionPlan> {
        Task<List<SubscriptionPlan>> FindAll();
        Task<SubscriptionPlan?> FindByBillingReference(BillingReference reference);
        Task<SubscriptionPlan?> FindByName(string name);
        Task DeleteAll();
    }
}
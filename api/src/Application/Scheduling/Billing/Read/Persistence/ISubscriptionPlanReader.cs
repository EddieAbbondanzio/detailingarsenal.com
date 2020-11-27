using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Application.Scheduling.Billing {
    public interface ISubscriptionPlanReader {
        Task<List<SubscriptionPlanReadModel>> ReadAll();
        Task<SubscriptionPlanReadModel?> ReadById(Guid id);
        Task<SubscriptionPlanReadModel> ReadDefault();
    }
}
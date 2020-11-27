using System;

namespace DetailingArsenal.Application.Scheduling.Billing {
    public class GetByIdSubscriptionPlanQuery : IAction {
        public Guid Id { get; }

        public GetByIdSubscriptionPlanQuery(Guid id) {
            Id = id;    
        }
    }
}
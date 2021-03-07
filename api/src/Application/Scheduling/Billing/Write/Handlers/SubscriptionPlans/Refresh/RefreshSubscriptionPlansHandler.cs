using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    [Authorization(Action = "refresh", Scope = "subscription-plans")]
    [DependencyInjection]
    public class RefreshSubscriptionPlansHandler : ActionHandler<RefreshSubscriptionPlansCommand> {
        ISubscriptionPlanRefresher refreshService;

        public RefreshSubscriptionPlansHandler(ISubscriptionPlanRefresher service) {
            this.refreshService = service;
        }

        public async override Task Execute(RefreshSubscriptionPlansCommand input, User? user) {
            await refreshService.RefreshPlans();
        }
    }
}
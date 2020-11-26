using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    [Authorization(Action = "refresh", Scope = "subscription-plans")]
    public class RefreshSubscriptionPlansHandler : ActionHandler<RefreshSubscriptionPlansCommand, CommandResult> {
        ISubscriptionPlanRefresher refreshService;

        public RefreshSubscriptionPlansHandler(ISubscriptionPlanRefresher service) {
            this.refreshService = service;
        }

        public async override Task<CommandResult> Execute(RefreshSubscriptionPlansCommand input, User? user) {
            await refreshService.RefreshPlans();
            return CommandResult.Success();
        }
    }
}
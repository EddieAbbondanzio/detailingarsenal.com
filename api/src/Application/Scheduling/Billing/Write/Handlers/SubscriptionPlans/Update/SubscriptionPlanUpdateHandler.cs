using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    [Authorization(Action = "update", Scope = "subscription-plans")]
    public class SubscriptionPlanUpdateHandler : ActionHandler<SubscriptionPlanUpdateCommand, CommandResult> {
        ISubscriptionPlanRepo repo;

        public SubscriptionPlanUpdateHandler(ISubscriptionPlanRepo repo) {
            this.repo = repo;
        }

        public async override Task<CommandResult> Execute(SubscriptionPlanUpdateCommand input, User? user) {
            var plan = await repo.FindById(input.Id);

            if (plan == null) {
                throw new EntityNotFoundException();
            }

            plan.Description = input.Description;
            plan.RoleId = input.RoleId;

            await repo.Update(plan);
            return CommandResult.Success();
        }
    }
}
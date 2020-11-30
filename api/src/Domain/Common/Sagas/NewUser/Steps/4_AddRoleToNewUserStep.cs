using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;
using DetailingArsenal.Domain.Users.Security;

namespace DetailingArsenal.Domain.Common {
    public class AddRoleToNewUserStep : SagaStep<string> {
        IRoleAssigner roleAssigner;

        public AddRoleToNewUserStep(IRoleAssigner roleAssigner) {
            this.roleAssigner = roleAssigner;
        }

        public async override Task Execute(SagaContext<string> context) {
            if (((IDictionary<string, object>)context.Data).ContainsKey("Plan")) {
                await roleAssigner.AddRole(context.Data.User, (Guid)context.Data.Plan.roleId);
            } else {
                await roleAssigner.AddRole(context.Data.User, "Free");
            }
        }

        public async override Task Compensate(SagaContext<string> context) {
            if (((IDictionary<string, object>)context.Data).ContainsKey("Plan")) {
                await roleAssigner.RemoveRole(context.Data.User, (Guid)context.Data.Plan.roleId);
            } else {
                await roleAssigner.RemoveRole(context.Data.User, "Free");
            }
        }
    }
}
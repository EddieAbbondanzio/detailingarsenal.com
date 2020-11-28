using System;
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
            await roleAssigner.AddRoleToUser(context.Data.User, context.Data.Plan.Name);
        }

        public async override Task Compensate(SagaContext<string> context) {
            await roleAssigner.RemoveRoleFromUser(context.Data.User, context.Data.Plan.Name);
        }
    }
}
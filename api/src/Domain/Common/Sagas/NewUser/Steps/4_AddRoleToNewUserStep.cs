using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;
using DetailingArsenal.Domain.Users.Security;

namespace DetailingArsenal.Domain.Common {
    public class AddRoleToNewUserStep : SagaStep<string> {
        IRoleService roleService;

        public AddRoleToNewUserStep(IRoleService roleService) {
            this.roleService = roleService;
        }

        public async override Task Execute(SagaContext<string> context) {
            //TODO: Refactor this out. It's not good to hardcode these but it works for now!
            var roles = await roleService.GetAll();

            var role = roles.Find(r => r.Name == context.Data.Plan.Name);

            if (role == null) {
                throw new InvalidOperationException($"No role with name ${context.Data.Plan.Name} exists");
            }

            await roleService.AddRoleToUser(role, context.Data.User);
        }

        public async override Task Compensate(SagaContext<string> context) {
            var role = (await roleService.GetAll()).Find(r => r.Name == context.Data.Plan.Name);
            await roleService.RemoveRoleFromUser(role, context.Data.User);
        }
    }
}
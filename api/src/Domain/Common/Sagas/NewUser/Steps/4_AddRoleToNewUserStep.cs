using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Common {
    public class AddRoleToNewUserStep : SagaStep<string> {
        IRoleService roleService;

        public AddRoleToNewUserStep(IRoleService roleService) {
            this.roleService = roleService;
        }

        public async override Task Execute(SagaContext<string> context) {
            //TODO: Refactor this out. It's not good to hardcode these but it works for now!
            var role = (await roleService.GetAll()).Find(r => r.Name == context.Data.Plan.Name);
            await roleService.AddRoleToUser(role, context.Data.User);
        }

        public async override Task Compensate(SagaContext<string> context) {
            var role = (await roleService.GetAll()).Find(r => r.Name == context.Data.Plan.Name);
            await roleService.RemoveRoleFromUser(role, context.Data.User);
        }
    }
}
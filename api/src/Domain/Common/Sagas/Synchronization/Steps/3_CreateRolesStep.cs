using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users.Security;

namespace DetailingArsenal.Domain.Common {
    public class CreateRolesStep : SagaStep {
        IRoleService roleService;

        public CreateRolesStep(IRoleService roleService) {
            this.roleService = roleService;
        }

        public async override Task Execute(SagaContext context) {
            var proRole = await roleService.TryGetByName("Pro");

            if (proRole == null) {
                await roleService.Create(new RoleCreate("Pro"));
            }

            var expiredRole = await roleService.TryGetByName("Expired");

            if (expiredRole == null) {
                await roleService.Create(new RoleCreate("Expired"));
            }
        }
    }
}
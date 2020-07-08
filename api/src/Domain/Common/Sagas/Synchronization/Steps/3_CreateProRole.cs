using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Security;

namespace DetailingArsenal.Domain.Common {
    public class CreateProRoleStep : SagaStep {
        IRoleService roleService;

        public CreateProRoleStep(IRoleService roleService) {
            this.roleService = roleService;
        }

        public async override Task Execute(SagaContext context) {
            var role = await roleService.TryGetByName("Pro");

            if (role == null) {
                await roleService.Create(new RoleCreate("Pro", new List<System.Guid>()));
            }
        }
    }
}
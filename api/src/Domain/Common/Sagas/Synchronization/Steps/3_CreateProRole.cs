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
            await roleService.Create(new CreateRole("Pro", new List<System.Guid>()));
        }
    }
}
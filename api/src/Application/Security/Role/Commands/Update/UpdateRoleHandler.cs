using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Security {
    [Validation(typeof(UpdateRoleValidator))]
    [Authorization(Action = "update", Scope = "roles")]
    public class UpdateRoleHandler : ActionHandler<UpdateRoleCommand, RoleView> {
        private IRoleService service;
        private IMapper mapper;

        public UpdateRoleHandler(IRoleService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<RoleView> Execute(UpdateRoleCommand input, User? user) {
            var r = await service.GetById(input.Id);
            await service.Update(r, new UpdateRole(
                input.Name,
                input.PermissionIds
            ));

            return mapper.Map<Role, RoleView>(r);
        }
    }
}
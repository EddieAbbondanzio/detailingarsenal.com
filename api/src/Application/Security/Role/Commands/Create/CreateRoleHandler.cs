using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;

namespace DetailingArsenal.Application.Security {
    [Validation(typeof(CreateRoleValidator))]
    [Authorization(Action = "create", Scope = "roles")]
    public class CreateRoleHandler : ActionHandler<CreateRoleCommand, RoleDto> {
        IRoleService service;
        private IMapper mapper;

        public CreateRoleHandler(IRoleService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<RoleDto> Execute(CreateRoleCommand input, User? user) {
            var r = await service.Create(new CreateRole(
                input.Name,
                input.PermissionIds
            ));

            return mapper.Map<Role, RoleDto>(r);
        }
    }
}
using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Validation(typeof(CreateRoleValidator))]
    [Authorization(Action = "create", Scope = "roles")]
    public class CreateRoleHandler : ActionHandler<RoleCreateCommand, RoleReadModel> {
        IRoleService service;
        private IMapper mapper;

        public CreateRoleHandler(IRoleService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<RoleReadModel> Execute(RoleCreateCommand input, User? user) {
            var r = await service.Create(new RoleCreate(
                input.Name,
                input.PermissionIds
            ));

            return mapper.Map<Role, RoleReadModel>(r);
        }
    }
}
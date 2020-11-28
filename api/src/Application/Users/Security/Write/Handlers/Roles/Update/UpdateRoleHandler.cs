using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Validation(typeof(UpdateRoleValidator))]
    [Authorization(Action = "update", Scope = "roles")]
    public class UpdateRoleHandler : ActionHandler<RoleUpdateCommand, RoleReadModel> {
        private IRoleService service;
        private IMapper mapper;

        public UpdateRoleHandler(IRoleService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<RoleReadModel> Execute(RoleUpdateCommand input, User? user) {
            var r = await service.GetById(input.Id);
            await service.Update(r, new RoleUpdate(
                input.Name,
                input.PermissionIds
            ));

            return mapper.Map<Role, RoleReadModel>(r);
        }
    }
}
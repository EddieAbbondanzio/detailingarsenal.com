using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Security {
    [Validation(typeof(CreatePermissionValidator))]
    [Authorization(Action = "create", Scope = "permissions")]
    public class CreatePermissionHandler : ActionHandler<CreatePermissionCommand, PermissionDto> {
        IPermissionService service;
        private IMapper mapper;

        public CreatePermissionHandler(IPermissionService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<PermissionDto> Execute(CreatePermissionCommand input, User? user) {
            var p = await service.Create(
                new CreatePermission(
                    input.Action,
                    input.Scope
                ),
                user!
            );
            return mapper.Map<Permission, PermissionDto>(p);
        }
    }
}
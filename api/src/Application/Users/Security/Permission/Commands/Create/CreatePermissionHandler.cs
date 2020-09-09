using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Validation(typeof(CreatePermissionValidator))]
    [Authorization(Action = "create", Scope = "permissions")]
    public class CreatePermissionHandler : ActionHandler<CreatePermissionCommand, PermissionView> {
        IPermissionService service;
        private IMapper mapper;

        public CreatePermissionHandler(IPermissionService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<PermissionView> Execute(CreatePermissionCommand input, User? user) {
            var p = await service.Create(
                new PermissionCreate(
                    input.Action,
                    input.Scope
                ),
                user!
            );
            return mapper.Map<Permission, PermissionView>(p);
        }
    }
}
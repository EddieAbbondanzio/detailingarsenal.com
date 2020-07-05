using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Security {
    [Validation(typeof(UpdatePermissionValidator))]
    [Authorization(Action = "update", Scope = "permissions")]
    public class UpdatePermissionHandler : ActionHandler<UpdatePermissionCommand, PermissionView> {
        IPermissionService service;
        IMapper mapper;

        public UpdatePermissionHandler(IPermissionService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<PermissionView> Execute(UpdatePermissionCommand input, User? user) {
            var p = await service.GetById(input.Id);
            await service.Update(p, new UpdatePermission(
                input.Action,
                input.Scope
            ));

            return mapper.Map<Permission, PermissionView>(p);
        }
    }
}
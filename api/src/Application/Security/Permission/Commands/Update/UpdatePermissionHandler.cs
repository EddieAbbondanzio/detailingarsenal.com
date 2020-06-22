using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;

namespace DetailingArsenal.Application.Security {
    [Validation(typeof(UpdatePermissionValidator))]
    [Authorization(Action = "update", Scope = "permissions")]
    public class UpdatePermissionHandler : ActionHandler<UpdatePermissionCommand, PermissionDto> {
        IPermissionService service;
        IMapper mapper;

        public UpdatePermissionHandler(IPermissionService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<PermissionDto> Execute(UpdatePermissionCommand input, User? user) {
            var p = await service.GetById(input.Id);
            await service.Update(p, new UpdatePermission(
                input.Action,
                input.Scope
            ));

            return mapper.Map<Permission, PermissionDto>(p);
        }
    }
}
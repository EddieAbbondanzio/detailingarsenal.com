using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Validation(typeof(UpdatePermissionValidator))]
    [Authorization(Action = "update", Scope = "permissions")]
    public class UpdatePermissionHandler : ActionHandler<PermissionUpdateCommand, PermissionReadModel> {
        IPermissionService service;
        IMapper mapper;

        public UpdatePermissionHandler(IPermissionService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<PermissionReadModel> Execute(PermissionUpdateCommand input, User? user) {
            var p = await service.GetById(input.Id);
            await service.Update(p, new PermissionUpdate(
                input.Action,
                input.Scope
            ));

            return mapper.Map<Permission, PermissionReadModel>(p);
        }
    }
}
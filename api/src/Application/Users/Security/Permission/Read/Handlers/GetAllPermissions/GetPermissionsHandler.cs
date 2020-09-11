
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Authorization(Action = "read", Scope = "permissions")]
    public class GetPermissionsHandler : ActionHandler<GetPermissionsQuery, List<PermissionView>> {
        IPermissionService service;
        private IMapper mapper;

        public GetPermissionsHandler(IPermissionService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<List<PermissionView>> Execute(GetPermissionsQuery input, User? user) {
            List<Permission> perms = await service.GetAll();
            return mapper.Map<List<Permission>, List<PermissionView>>(perms);
        }
    }
}
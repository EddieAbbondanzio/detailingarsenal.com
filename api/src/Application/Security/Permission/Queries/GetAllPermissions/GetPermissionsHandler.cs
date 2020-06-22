
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;

namespace DetailingArsenal.Application.Security {
    [Authorization(Action = "read", Scope = "permissions")]
    public class GetPermissionsHandler : ActionHandler<GetPermissionsQuery, List<PermissionDto>> {
        private IPermissionRepo repo;
        private IMapper mapper;

        public GetPermissionsHandler(IPermissionRepo repo, IMapper mapper) {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async override Task<List<PermissionDto>> Execute(GetPermissionsQuery input, User? user) {
            List<Permission> perms = await repo.FindAll();
            return mapper.Map<List<Permission>, List<PermissionDto>>(perms);
        }
    }
}
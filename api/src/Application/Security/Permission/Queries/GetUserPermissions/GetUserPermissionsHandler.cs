using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    [Authorization]
    public class GetUserPermissionsHandler : ActionHandler<GetUserPermissionsQuery, List<PermissionDto>> {
        private IRoleRepo roleRepo;
        private IPermissionRepo permissionRepo;
        private IMapper mapper;

        public GetUserPermissionsHandler(IRoleRepo roleRepo, IPermissionRepo permissionRepo, IMapper mapper) {
            this.roleRepo = roleRepo;
            this.permissionRepo = permissionRepo;
            this.mapper = mapper;
        }

        public async override Task<List<PermissionDto>> Execute(GetUserPermissionsQuery input, User? user) {
            var roles = await roleRepo.FindForUser(user!);
            var perms = await permissionRepo.FindForRoles(roles);

            return mapper.Map<List<Permission>, List<PermissionDto>>(perms);
        }
    }
}
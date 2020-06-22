using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;

namespace DetailingArsenal.Application.Security {
    [Authorization]
    public class GetUserPermissionsHandler : ActionHandler<GetUserPermissionsQuery, List<PermissionDto>> {
        IRoleRepo roleRepo;
        IPermissionService service;
        IMapper mapper;

        public GetUserPermissionsHandler(IRoleRepo roleRepo, IPermissionService service, IMapper mapper) {
            this.roleRepo = roleRepo;
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<List<PermissionDto>> Execute(GetUserPermissionsQuery input, User? user) {
            var roles = await roleRepo.FindForUser(user!);
            var perms = await service.GetForRoles(roles);

            return mapper.Map<List<Permission>, List<PermissionDto>>(perms);
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

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
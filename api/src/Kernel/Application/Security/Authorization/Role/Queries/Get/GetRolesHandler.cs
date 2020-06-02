
using System.Collections.Generic;
using System.Threading.Tasks;

[Authorization(Action = "read", Scope = "roles")]
public class GetRolesHandler : ActionHandler<GetRolesQuery, List<RoleDto>> {
    private IRoleRepo repo;
    private IMapper mapper;

    public GetRolesHandler(IRoleRepo repo, IMapper mapper) {
        this.repo = repo;
        this.mapper = mapper;
    }

    public async override Task<List<RoleDto>> Execute(GetRolesQuery input, User? user) {
        List<Role> perms = await repo.FindAll();
        return mapper.Map<List<Role>, List<RoleDto>>(perms);
    }
}
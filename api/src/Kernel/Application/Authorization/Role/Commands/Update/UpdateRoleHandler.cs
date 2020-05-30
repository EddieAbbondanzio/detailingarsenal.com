using System;
using System.Threading.Tasks;

public class UpdateRoleHandler : ActionHandler<UpdateRoleCommand, RoleDto> {
    private IRoleRepo repo;
    private IMapper mapper;

    public UpdateRoleHandler(IRoleRepo repo, IMapper mapper) {
        this.repo = repo;
        this.mapper = mapper;
    }

    protected async override Task<RoleDto> Execute(UpdateRoleCommand input, User? user) {
        var p = await repo.FindById(input.Id);

        if (p == null) {
            throw new EntityNotFoundException();
        }

        p.Name = input.Name;
        p.PermissionIds = input.PermissionIds;

        await repo.Update(p);
        return mapper.Map<Role, RoleDto>(p);
    }
}
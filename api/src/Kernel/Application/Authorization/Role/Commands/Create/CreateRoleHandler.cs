using System;
using System.Threading.Tasks;

public class CreateRoleHandler : ActionHandler<CreateRoleCommand, RoleDto> {
    private IRoleRepo repo;
    private IMapper mapper;

    public CreateRoleHandler(IRoleRepo repo, IMapper mapper) {
        this.repo = repo;
        this.mapper = mapper;
    }

    protected async override Task<RoleDto> Execute(CreateRoleCommand input, User? user) {
        var p = new Role() {
            Id = Guid.NewGuid(),
            Name = input.Name,
            PermissionIds = input.PermissionIds
        };

        await repo.Add(p);
        return mapper.Map<Role, RoleDto>(p);
    }
}
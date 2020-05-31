using System;
using System.Threading.Tasks;

public class UpdateRoleHandler : ActionHandler<UpdateRoleCommand, RoleDto> {
    private RoleNameUniqueSpecification specification;
    private IRoleRepo repo;
    private IMapper mapper;

    public UpdateRoleHandler(RoleNameUniqueSpecification specification, IRoleRepo repo, IMapper mapper) {
        this.specification = specification;
        this.repo = repo;
        this.mapper = mapper;
    }

    protected async override Task<RoleDto> Execute(UpdateRoleCommand input, User? user) {
        var r = await repo.FindById(input.Id);

        if (r == null) {
            throw new EntityNotFoundException();
        }

        await specification.CheckAndThrow(r);

        r.Name = input.Name;
        r.PermissionIds = input.PermissionIds;

        await repo.Update(r);
        return mapper.Map<Role, RoleDto>(r);
    }
}
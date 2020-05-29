using System.Threading.Tasks;

public class UpdatePermissionHandler : ActionHandler<UpdatePermissionCommand, PermissionDto> {
    private PermissionUniqueSpecification specification;
    private IPermissionRepo repo;
    private IMapper mapper;

    public UpdatePermissionHandler(PermissionUniqueSpecification specification, IPermissionRepo repo, IMapper mapper) {
        this.specification = specification;
        this.repo = repo;
        this.mapper = mapper;
    }

    protected async override Task<PermissionDto> Execute(UpdatePermissionCommand input, User? user) {
        var p = await repo.FindById(input.Id);

        if (p == null) {
            throw new EntityNotFoundException();
        }

        p.Action = input.Action;
        p.Scope = input.Scope;

        await specification.CheckAndThrow(p);

        await repo.Update(p);
        return mapper.Map<Permission, PermissionDto>(p);
    }
}
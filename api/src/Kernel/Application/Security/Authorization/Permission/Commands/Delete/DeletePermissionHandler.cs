using System.Threading.Tasks;

public class DeletePermissionHandler : ActionHandler<DeletePermissionCommand> {
    private PermissionNotInUseSpecification specification;
    private IPermissionRepo repo;

    public DeletePermissionHandler(PermissionNotInUseSpecification specification, IPermissionRepo repo) {
        this.specification = specification;
        this.repo = repo;
    }

    protected async override Task Execute(DeletePermissionCommand input, User? user) {
        var p = await repo.FindById(input.Id);

        if (p == null) {
            throw new EntityNotFoundException();
        }

        await specification.CheckAndThrow(p);

        await repo.Delete(p);
    }
}
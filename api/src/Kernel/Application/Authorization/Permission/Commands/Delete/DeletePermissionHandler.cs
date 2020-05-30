using System.Threading.Tasks;

public class DeletePermissionHandler : ActionHandler<DeletePermissionCommand> {
    private IPermissionRepo repo;

    public DeletePermissionHandler(IPermissionRepo repo) {
        this.repo = repo;
    }

    protected async override Task Execute(DeletePermissionCommand input, User? user) {
        var p = await repo.FindById(input.Id);

        if (p == null) {
            throw new EntityNotFoundException();
        }

        await repo.Delete(p);
    }
}
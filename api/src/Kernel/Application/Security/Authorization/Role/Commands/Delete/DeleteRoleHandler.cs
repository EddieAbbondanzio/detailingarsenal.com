using System.Threading.Tasks;

public class DeleteRoleHandler : ActionHandler<DeleteRoleCommand> {
    private IRoleRepo repo;

    public DeleteRoleHandler(IRoleRepo repo) {
        this.repo = repo;
    }

    public async override Task Execute(DeleteRoleCommand input, User? user) {
        var r = await repo.FindById(input.Id);

        if (r == null) {
            return;
        }

        await repo.Delete(r);
    }
}
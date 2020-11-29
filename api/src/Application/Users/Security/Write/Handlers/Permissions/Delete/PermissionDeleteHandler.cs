using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Authorization(Action = "delete", Scope = "permissions")]
    public class PermissionDeleteHandler : ActionHandler<PermissionDeleteCommand> {
        IPermissionRepo repo;
        PermissionNotInUseSpecification spec;

        public PermissionDeleteHandler(IPermissionRepo repo, PermissionNotInUseSpecification spec) {
            this.repo = repo;
            this.spec = spec;
        }

        public async override Task Execute(PermissionDeleteCommand input, User? user) {
            var p = await repo.FindById(input.Id) ?? throw new EntityNotFoundException();

            await spec.CheckAndThrow(p);

            await repo.Delete(p);
        }
    }
}
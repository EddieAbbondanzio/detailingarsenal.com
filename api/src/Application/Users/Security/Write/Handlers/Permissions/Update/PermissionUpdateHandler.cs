using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Validation(typeof(UpdatePermissionValidator))]
    [Authorization(Action = "update", Scope = "permissions")]
    [DependencyInjection]
    public class PermissionUpdateHandler : ActionHandler<PermissionUpdateCommand> {
        IPermissionRepo repo;
        PermissionUniqueSpecification spec;

        public PermissionUpdateHandler(IPermissionRepo repo, PermissionUniqueSpecification spec) {
            this.repo = repo;
            this.spec = spec;
        }

        public async override Task Execute(PermissionUpdateCommand input, User? user) {
            var p = await repo.FindById(input.Id) ?? throw new EntityNotFoundException();

            p.Action = input.Action;
            p.Scope = input.Scope;

            await spec.CheckAndThrow(p);

            await repo.Update(p);
        }
    }
}

using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Authorization(Action = "delete", Scope = "roles")]
    public class DeleteRoleHandler : ActionHandler<RoleDeleteCommand, CommandResult> {
        IRoleRepo repo;
        RoleNotInUseSpecification spec;

        public DeleteRoleHandler(IRoleRepo repo, RoleNotInUseSpecification spec) {
            this.repo = repo;
            this.spec = spec;
        }

        public async override Task<CommandResult> Execute(RoleDeleteCommand input, User? user) {
            var r = await repo.FindById(input.Id) ?? throw new EntityNotFoundException();

            await spec.CheckAndThrow(r);

            await repo.Delete(r);
            return CommandResult.Success();
        }
    }
}
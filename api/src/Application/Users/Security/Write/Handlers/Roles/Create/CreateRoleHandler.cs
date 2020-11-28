using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Validation(typeof(CreateRoleValidator))]
    [Authorization(Action = "create", Scope = "roles")]
    public class CreateRoleHandler : ActionHandler<RoleCreateCommand, CommandResult> {
        IRoleRepo repo;
        RoleNameUniqueSpecification spec;

        public CreateRoleHandler(IRoleRepo repo, RoleNameUniqueSpecification spec) {
            this.repo = repo;
            this.spec = spec;
        }

        public async override Task<CommandResult> Execute(RoleCreateCommand input, User? user) {
            var r = new Role(input.Name, input.PermissionIds);

            await spec.CheckAndThrow(r);
            await repo.Add(r);

            return CommandResult.Success();
        }
    }
}
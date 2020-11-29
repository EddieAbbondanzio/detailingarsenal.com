
using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Validation(typeof(UpdateRoleValidator))]
    [Authorization(Action = "update", Scope = "roles")]
    public class UpdateRoleHandler : ActionHandler<RoleUpdateCommand> {
        IRoleRepo repo;
        RoleNameUniqueSpecification spec;

        public UpdateRoleHandler(IRoleRepo repo, RoleNameUniqueSpecification spec) {
            this.repo = repo;
            this.spec = spec;
        }

        public async override Task Execute(RoleUpdateCommand input, User? user) {
            var r = await repo.FindById(input.Id) ?? throw new EntityNotFoundException();

            r.Name = input.Name;
            r.PermissionIds = input.PermissionIds;

            await spec.CheckAndThrow(r);

            await repo.Update(r);
        }
    }
}
using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Validation(typeof(CreatePermissionValidator))]
    [Authorization(Action = "create", Scope = "permissions")]
    public class PermissionCreateHandler : ActionHandler<PermissionCreateCommand, Guid> {
        PermissionUniqueSpecification spec;
        IPermissionRepo repo;

        public PermissionCreateHandler(PermissionUniqueSpecification spec, IPermissionRepo repo) {
            this.spec = spec;
            this.repo = repo;
        }

        public async override Task<Guid> Execute(PermissionCreateCommand input, User? user) {
            var p = new Permission(input.Action, input.Scope);

            await spec.CheckAndThrow(p);

            await repo.Add(p);
            return p.Id;
        }
    }
}

using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;
using System.Linq;

namespace DetailingArsenal.Application.Users.Security {
    [Validation(typeof(UpdateRoleValidator))]
    [Authorization(Action = "update", Scope = "roles")]
    [DependencyInjection]
    public class UpdateRoleHandler : ActionHandler<RoleUpdateCommand> {
        IRoleRepo repo;
        RoleNameUniqueSpecification nameUniqueSpec;
        RolePermissionsDistinctSpecification permissionsDistinctSpec;

        public UpdateRoleHandler(IRoleRepo repo, RoleNameUniqueSpecification nameUniqueSpec, RolePermissionsDistinctSpecification permissionsDistinctSpec) {
            this.repo = repo;
            this.nameUniqueSpec = nameUniqueSpec;
            this.permissionsDistinctSpec = permissionsDistinctSpec;
        }

        public async override Task Execute(RoleUpdateCommand input, User? user) {
            var r = await repo.FindById(input.Id) ?? throw new EntityNotFoundException();

            r.Name = input.Name;
            r.PermissionIds = input.PermissionIds.Distinct().ToList();

            await nameUniqueSpec.CheckAndThrow(r);
            await permissionsDistinctSpec.CheckAndThrow(r);

            await repo.Update(r);
        }
    }
}
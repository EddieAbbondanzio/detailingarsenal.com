using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;
using System.Linq;

namespace DetailingArsenal.Application.Users.Security {
    [Validation(typeof(CreateRoleValidator))]
    [Authorization(Action = "create", Scope = "roles")]
    [DependencyInjection]
    public class CreateRoleHandler : ActionHandler<RoleCreateCommand, Guid> {
        IRoleRepo repo;
        RoleNameUniqueSpecification uniqueNameSpec;
        RolePermissionsDistinctSpecification permissionsDistinctSpec;

        public CreateRoleHandler(IRoleRepo repo, RoleNameUniqueSpecification uniqueNameSpec, RolePermissionsDistinctSpecification permissionsDistinctSpec) {
            this.repo = repo;
            this.uniqueNameSpec = uniqueNameSpec;
            this.permissionsDistinctSpec = permissionsDistinctSpec;
        }

        public async override Task<Guid> Execute(RoleCreateCommand input, User? user) {
            var r = new Role(input.Name, input.PermissionIds.Distinct().ToList());

            await uniqueNameSpec.CheckAndThrow(r);
            await permissionsDistinctSpec.CheckAndThrow(r);
            await repo.Add(r);

            return r.Id;
        }
    }
}
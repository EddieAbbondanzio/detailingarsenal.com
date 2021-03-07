using System.Linq;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Users.Security {
    /// <summary>
    /// Check to see if every permission id on a role is unique to prevent duplicates.
    /// </summary>
    [DependencyInjection]
    public class RolePermissionsDistinctSpecification : Specification<Role> {
        protected override Task<SpecificationResult> IsSatisfied(Role entity) {
            if (entity.PermissionIds.Distinct().Count() == entity.PermissionIds.Count) {
                return Task.FromResult(new SpecificationResult(true));
            } else {
                return Task.FromResult(new SpecificationResult(false, $"Duplicate permissions exist in role {entity.Name}"));
            }
        }
    }
}
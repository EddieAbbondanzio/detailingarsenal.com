using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Users.Security {
    public class RoleNotInUseSpecification : Specification<Role> {
        private IRoleRepo repo;

        public RoleNotInUseSpecification(IRoleRepo repo) {
            this.repo = repo;
        }

        protected async override Task<SpecificationResult> IsSatisfied(Role entity) {
            if (await repo.IsRoleInUse(entity)) {
                return new SpecificationResult(false, $"Role {entity.Name} is in use.");
            } else {
                return new SpecificationResult(true);
            }
        }
    }
}
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Security {
    public class PermissionUniqueSpecification : Specification<Permission> {
        private IPermissionRepo repo;

        public PermissionUniqueSpecification(IPermissionRepo repo) {
            this.repo = repo;
        }

        protected async override Task<SpecificationResult> IsSatisfied(Permission entity) {
            var existing = await repo.Find(entity.Action, entity.Scope);

            if (existing == null || existing.Id == entity.Id) {
                return new SpecificationResult(true);
            } else {
                return new SpecificationResult(false, $"Permission {entity.ToString()} is already defined.");
            }
        }
    }
}
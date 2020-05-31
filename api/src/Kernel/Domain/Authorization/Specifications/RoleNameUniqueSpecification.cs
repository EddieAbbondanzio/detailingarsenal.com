using System.Threading.Tasks;

public class RoleNameUniqueSpecification : Specification<Role> {
    private IRoleRepo repo;

    public RoleNameUniqueSpecification(IRoleRepo repo) {
        this.repo = repo;
    }

    protected async override Task<SpecificationResult> IsSatisfied(Role entity) {
        var existing = await repo.Find(entity.Name);

        if (existing == null || existing.Id == entity.Id) {
            return new SpecificationResult(true);
        } else {
            return new SpecificationResult(false, $"Role {entity.Name} is already defined.");
        }
    }
}
using System.Threading.Tasks;

public class PermissionNotInUseSpecification : Specification<Permission> {
    private IPermissionRepo repo;

    public PermissionNotInUseSpecification(IPermissionRepo repo) {
        this.repo = repo;
    }

    protected async override Task<SpecificationResult> IsSatisfied(Permission entity) {
        var inUse = await repo.IsInUse(entity);

        if (inUse) {
            return new SpecificationResult(false, "Permission is in use.");
        } else {
            return new SpecificationResult(true);
        }
    }
}
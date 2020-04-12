using System.Linq;
using System.Threading.Tasks;

public class OrSpecification<T> : Specification<T> {
    private Specification<T> left;
    private Specification<T> right;

    public OrSpecification(Specification<T> left, Specification<T> right) {
        this.left = left;
        this.right = right;
    }

    protected async override Task<SpecificationResult> IsSatisfied(T entity) {
        var results = await Task.WhenAll(left.Check(entity), right.Check(entity));

        if (results[0].IsSatisfied || results[1].IsSatisfied) {
            return new SpecificationResult(true);
        } else {
            return new SpecificationResult(false, results.Where(r => !r.IsSatisfied).Select(r => r.Messages[0]).ToList());
        }
    }
}
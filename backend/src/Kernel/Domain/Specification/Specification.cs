using System.Threading.Tasks;

public abstract class Specification<T> {
    public async Task<SpecificationResult> Check(T entity) {
        return await IsSatisfied(entity);
    }

    public async Task<SpecificationResult> CheckAndThrow(T entity) {
        var result = await IsSatisfied(entity);

        if (!result.IsSatisfied) {
            throw new SpecificationException(result);
        }

        return result;
    }

    protected abstract Task<SpecificationResult> IsSatisfied(T entity);

    protected SpecificationResult Satisfied() => new SpecificationResult(true);

    public Specification<T> And(Specification<T> other) {
        return new AndSpecification<T>(this, other);
    }

    public Specification<T> Or(Specification<T> other) {
        return new OrSpecification<T>(this, other);
    }
}
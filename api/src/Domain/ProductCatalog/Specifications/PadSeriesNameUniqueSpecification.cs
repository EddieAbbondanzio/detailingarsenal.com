using System.Threading.Tasks;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeriesNameUniqueSpecification : Specification<PadSeries> {
        public IPadSeriesRepo repo;

        public PadSeriesNameUniqueSpecification(IPadSeriesRepo repo) {
            this.repo = repo;
        }

        protected async override Task<SpecificationResult> IsSatisfied(PadSeries entity) {
            var existingPadSeries = await repo.FindByName(entity.Name);

            if (existingPadSeries != null && existingPadSeries.Id != entity.Id) {
                return new SpecificationResult(false, $"Pad series name {existingPadSeries.Name} is already in use.");
            }

            return new SpecificationResult(true);
        }
    }
}
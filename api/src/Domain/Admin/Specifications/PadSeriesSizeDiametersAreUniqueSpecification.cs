using System.Linq;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Admin.ProductCatalog {
    public class PadSeriesPadSizeDiametersAreUniqueSpecification : Specification<PadSeries> {

#pragma warning disable 1998
        protected async override Task<SpecificationResult> IsSatisfied(PadSeries series) {
            var duplicate = series.Sizes.GroupBy(s => s.Diameter).Any(g => g.Count() > 1);

            if (duplicate) {
                return new SpecificationResult(false, "Duplicate pad sizes with the same diameter exist.");
            } else {
                return new SpecificationResult(true);
            }
        }
#pragma warning restore 1998
    }
}
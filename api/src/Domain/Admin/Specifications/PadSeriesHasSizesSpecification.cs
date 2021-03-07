using System.Linq;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Admin.ProductCatalog {
    [DependencyInjection]
    public class PadSeriesHasSizesSpecification : Specification<PadSeries> {
#pragma warning disable 1998
        protected async override Task<SpecificationResult> IsSatisfied(PadSeries series) {
            if (series.Sizes.Count == 0) {
                return new SpecificationResult(false, "Pad series has no sizes.");
            } else {
                return new SpecificationResult(true);
            }
        }
#pragma warning restore 1998
    }
}
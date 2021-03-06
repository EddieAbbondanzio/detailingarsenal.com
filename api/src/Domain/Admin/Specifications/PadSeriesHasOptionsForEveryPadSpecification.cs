using System.Linq;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Admin.ProductCatalog {
    public class PadSeriesHasOptionsForEveryPadSpecification : Specification<PadSeries> {
#pragma warning disable 1998
        protected async override Task<SpecificationResult> IsSatisfied(PadSeries series) {
            if (series.Pads.Any(c => c.Options.Count == 0)) {
                return new SpecificationResult(false, "Pad series has a color with no options");
            } else {
                return new SpecificationResult(true);
            }
        }
#pragma warning restore 1998
    }
}
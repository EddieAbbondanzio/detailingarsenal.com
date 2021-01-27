using System.Linq;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeriesHasPadsSpecification : Specification<PadSeries> {
#pragma warning disable 1998
        protected async override Task<SpecificationResult> IsSatisfied(PadSeries series) {
            if (series.Pads.Count == 0) {
                return new SpecificationResult(false, "Pad series has no colors.");
            } else {
                return new SpecificationResult(true);
            }
        }
#pragma warning restore 1998
    }
}
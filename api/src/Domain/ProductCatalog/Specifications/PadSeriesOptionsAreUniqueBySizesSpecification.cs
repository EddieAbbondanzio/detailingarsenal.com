using System.Linq;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeriesOptionsAreUniqueBySizesSpecification : Specification<PadSeries> {
#pragma warning disable 1998
        protected async override Task<SpecificationResult> IsSatisfied(PadSeries series) {
            foreach (Pad color in series.Pads) {
                if (color.Options.GroupBy(o => o.PadSizeId).Any(g => g.Count() > 1)) {
                    return new SpecificationResult(false, $"Pad color {color.Name} has more than 1 option with the same size specified.");
                }
            }

            return new SpecificationResult(true);
        }
#pragma warning restore 1998
    }
}
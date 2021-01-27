using System.Threading.Tasks;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadSeriesCreateOrUpdateCompositeSpecification : Specification<PadSeries> {
        Specification<PadSeries> groupedSpec;

        /*
        *
        * Kinda gross, but we need a type to inject it into the IoC container.
        *
        */

        public PadSeriesCreateOrUpdateCompositeSpecification(
            PadSeriesHasPadsSpecification hasColors,
            PadSeriesHasSizesSpecification hasSizes,
            PadSeriesHasOptionsForEveryPadSpecification hasOptions,
            PadSeriesOptionsAreUniqueBySizesSpecification optionsAreUnique,
            PadSeriesPadSizeDiametersAreUniqueSpecification sizeDiametersAreUnique) {
            groupedSpec = new AndSpecification<PadSeries>(
                new AndSpecification<PadSeries>(hasColors, hasOptions),
                new AndSpecification<PadSeries>(hasSizes, sizeDiametersAreUnique)
            );
        }

        protected async override Task<SpecificationResult> IsSatisfied(PadSeries entity) => await groupedSpec.Check(entity);
    }
}
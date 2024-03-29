using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Admin.ProductCatalog {
    [DependencyInjection]
    public class PadSeriesCreateOrUpdateCompositeSpecification : Specification<PadSeries> {
        Specification<PadSeries> groupedSpec;

        /*
        *
        * Kinda gross, but we need a type to inject it into the IoC container.
        *
        */

        public PadSeriesCreateOrUpdateCompositeSpecification(
            PadSeriesNameUniqueSpecification uniqueName,
            PadSeriesHasPadsSpecification hasColors,
            PadSeriesHasSizesSpecification hasSizes,
            PadSeriesHasOptionsForEveryPadSpecification hasOptions,
            PadSeriesOptionsAreUniqueBySizesSpecification optionsAreUnique,
            PadSeriesPadSizeDiametersAreUniqueSpecification sizeDiametersAreUnique) {
            groupedSpec = new AndSpecification<PadSeries>(
               new AndSpecification<PadSeries>(uniqueName, new AndSpecification<PadSeries>(hasColors, hasOptions)),
                new AndSpecification<PadSeries>(hasSizes, sizeDiametersAreUnique)
            );
        }

        protected async override Task<SpecificationResult> IsSatisfied(PadSeries entity) => await groupedSpec.Check(entity);
    }
}
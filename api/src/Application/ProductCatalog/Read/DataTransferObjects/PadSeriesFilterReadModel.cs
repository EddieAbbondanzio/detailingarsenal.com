using System.Collections.Generic;
using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadSeriesFilterReadModel : IDataTransferObject {
        public IEnumerable<PadSeriesFilterBrandReadModel> Brands { get; }
        public IEnumerable<PadSeriesFilterSeriesReadModel> Series { get; }

        public PadSeriesFilterReadModel(IEnumerable<PadSeriesFilterBrandReadModel> brands, IEnumerable<PadSeriesFilterSeriesReadModel> series) {
            Brands = brands;
            Series = series;
        }
    }
    public record PadSeriesFilterBrandReadModel(Guid Id, string Name) : IDataTransferObject;
    public record PadSeriesFilterSeriesReadModel(Guid Id, string Name, string BrandName) : IDataTransferObject;
}
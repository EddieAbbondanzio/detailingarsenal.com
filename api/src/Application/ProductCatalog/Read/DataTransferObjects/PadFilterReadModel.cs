using System.Collections.Generic;
using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadFilterReadModel : IDataTransferObject {
        public IEnumerable<PadFilterBrandReadModel> Brands { get; }
        public IEnumerable<PadFilterSeriesReadModel> Series { get; }

        public PadFilterReadModel(IEnumerable<PadFilterBrandReadModel> brands, IEnumerable<PadFilterSeriesReadModel> series) {
            Brands = brands;
            Series = series;
        }
    }
    public record PadFilterBrandReadModel(Guid Id, string Name) : IDataTransferObject;
    public record PadFilterSeriesReadModel(Guid Id, string Name, string BrandName) : IDataTransferObject;
}
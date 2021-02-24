using System.Collections.Generic;
using System;

namespace DetailingArsenal.Application.ProductCatalog {
    public record PadSeriesFilterReadModel : IDataTransferObject {
        public IEnumerable<PadSeriesFilterOptionReadModel> Brands { get; }
        public IEnumerable<PadSeriesFilterOptionReadModel> Series { get; }

        public PadSeriesFilterReadModel(IEnumerable<PadSeriesFilterOptionReadModel> brands, IEnumerable<PadSeriesFilterOptionReadModel> series) {
            Brands = brands;
            Series = series;
        }
    }
    public record PadSeriesFilterOptionReadModel(Guid Id, string Name) : IDataTransferObject;
}
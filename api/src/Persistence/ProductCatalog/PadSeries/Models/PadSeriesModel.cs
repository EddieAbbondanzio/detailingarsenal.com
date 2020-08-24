using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class PadSeriesModel : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
    }
}
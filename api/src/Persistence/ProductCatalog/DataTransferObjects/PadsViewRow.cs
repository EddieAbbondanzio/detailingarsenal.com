using System;

namespace DetailingArsenal.Persistence.ProductCatalog {
    internal class PadsViewRow : IDataTransferObject {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid PadSeriesId { get; set; }
        public string PadSeriesName { get; set; } = null!;
        public Guid BrandId { get; set; }
        public string BrandName { get; set; } = null!;
        public PadCategoryBitwise Category { get; set; }
        public PolisherTypeBitwise PolisherTypes { get; set; }
        public string Material { get; set; } = null!;
        public string Texture { get; set; } = null!;
        public bool HasCenterHole { get; set; }
        public int? Cut { get; set; }
        public int? Finish { get; set; }
        public float? Stars { get; set; }
        public int ReviewCount { get; set; }
    }
}

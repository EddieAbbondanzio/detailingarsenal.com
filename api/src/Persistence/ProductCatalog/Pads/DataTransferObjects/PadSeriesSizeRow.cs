using System;

namespace DetailingArsenal.Persistence.ProductCatalog {
    /// <summary>
    /// pad_size row model
    /// </summary>
    public class PadSeriesSizeRow : IDataTransferObject {
        public Guid PadSeriesId { get; set; }
        public float Diameter { get; set; }
        public float Thickness { get; set; }
        public string PartNumber { get; set; } = null!;
    }
}
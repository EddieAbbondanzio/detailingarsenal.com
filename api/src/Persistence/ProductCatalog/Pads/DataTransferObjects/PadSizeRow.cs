using System;

namespace DetailingArsenal.Persistence.ProductCatalog {
    /// <summary>
    /// pad_size row model
    /// </summary>
    public class PadSizeRow : IDataTransferObject {
        public Guid PadId { get; set; }
        public float Diameter { get; set; }
        public float Thickness { get; set; }
        public string PartNumber { get; set; } = null!;
    }
}
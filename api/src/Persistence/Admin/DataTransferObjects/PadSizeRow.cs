using System;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog {
    /// <summary>
    /// pad_size row model
    /// </summary>
    internal class PadSizeRow : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid PadSeriesId { get; set; }
        public float DiameterAmount { get; set; }
        public string DiameterUnit { get; set; } = null!;
        public float? ThicknessAmount { get; set; }
        public string? ThicknessUnit { get; set; }
    }
}
using System;

namespace DetailingArsenal.Persistence.ProductCatalog {
    /// <summary>
    /// pad_size row model
    /// </summary>
    internal class PadSizeRow : IDataTransferObject {
        /// <summary>
        /// Used by pad options to reference a specific pad color / pad size combo.
        /// </summary>
        public Guid Id { get; set; }
        public Guid PadSeriesId { get; set; }
        public float Diameter { get; set; }
        public float? Thickness { get; set; }
    }
}
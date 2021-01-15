using System;

namespace DetailingArsenal.Api.ProductCatalog {
    public class PadSizeUpdateRaw : IDataTransferObject {
        public Guid Id { get; set; }
        public MeasurementRaw Diameter { get; set; } = null;
        public MeasurementRaw? Thickness { get; set; }
    }
}
namespace DetailingArsenal.Api.ProductCatalog {
    public class PadSizeCreateRaw : IDataTransferObject {
        public MeasurementRaw Diameter { get; set; } = null!;
        public MeasurementRaw? Thickness { get; set; }

    }
}
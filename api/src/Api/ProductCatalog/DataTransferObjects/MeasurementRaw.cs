namespace DetailingArsenal.Api.ProductCatalog {
    public class MeasurementRaw : IDataTransferObject {
        public float Amount { get; set; }
        public string Unit { get; set; } = null!;
    }
}
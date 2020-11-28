namespace DetailingArsenal.Api.ProductCatalog {
    public class PadSeriesSizeRaw : IDataTransferObject {
        public float Diameter { get; set; }
        public float? Thickness { get; set; }
        public string? PartNumber { get; set; }
    }
}
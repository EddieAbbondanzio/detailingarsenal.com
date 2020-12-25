namespace DetailingArsenal.Api.ProductCatalog {
    public class PadOptionCreate : IDataTransferObject {
        public int PadSizeIndex { get; set; }
        public string? PartNumber { get; set; }
    }
}
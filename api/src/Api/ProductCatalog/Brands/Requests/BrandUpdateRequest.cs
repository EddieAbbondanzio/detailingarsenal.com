namespace DetailingArsenal.Api.ProductCatalog {
    public class BrandUpdateRequest : IDataTransferObject {
        public string Name { get; set; } = null!;
    }
}
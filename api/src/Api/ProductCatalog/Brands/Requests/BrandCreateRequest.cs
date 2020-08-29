namespace DetailingArsenal.Api.ProductCatalog {
    public class BrandCreateRequest : IDataTransferObject {
        public string Name { get; set; } = null!;
    }
}
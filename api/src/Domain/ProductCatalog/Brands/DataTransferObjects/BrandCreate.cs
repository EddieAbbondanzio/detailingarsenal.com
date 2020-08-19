namespace DetailingArsenal.Domain.ProductCatalog {
    public class BrandCreate : IDataTransferObject {
        public string Name { get; }

        public BrandCreate(string name) {
            Name = name;
        }
    }
}
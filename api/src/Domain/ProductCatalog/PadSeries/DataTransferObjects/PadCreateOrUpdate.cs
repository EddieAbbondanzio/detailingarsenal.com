namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadCreateOrUpdate : IDataTransferObject {
        public string Name { get; }
        public PadCategory Category { get; }
        public BinaryImage? Image { get; }

        public PadCreateOrUpdate(string name, PadCategory category, BinaryImage? image) {
            Name = name;
            Category = category;
            Image = image;
        }
    }
}
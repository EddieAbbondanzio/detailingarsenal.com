namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadCreate : IDataTransferObject {
        public string Name { get; }
        public PadCategory Category { get; }
        public byte[]? Image { get; }

        public PadCreate(string name, PadCategory category, byte[]? image) {
            Name = name;
            Category = category;
            Image = image;
        }
    }
}
namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadUpdate : IDataTransferObject {
        public string Name { get; }
        public PadCategory Category { get; }
        public byte[]? Image { get; }

        public PadUpdate(string name, PadCategory category, byte[]? image) {
            Name = name;
            Category = category;
            Image = image;
        }
    }
}
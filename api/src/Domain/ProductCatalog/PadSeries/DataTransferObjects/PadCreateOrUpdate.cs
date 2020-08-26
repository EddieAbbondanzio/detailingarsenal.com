namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadCreateOrUpdate : IDataTransferObject {
        public string Name { get; }
        public PadCategory Category { get; }
        public Base64Image? Image { get; }

        public PadCreateOrUpdate(string name, PadCategory category, Base64Image? image) {
            Name = name;
            Category = category;
            Image = image;
        }
    }
}
namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadCreateOrUpdate : IDataTransferObject {
        public string Name { get; }
        public PadCategory Category { get; }
        public DataUrlImage? Image { get; }

        public PadCreateOrUpdate(string name, PadCategory category, DataUrlImage? image) {
            Name = name;
            Category = category;
            Image = image;
        }
    }
}
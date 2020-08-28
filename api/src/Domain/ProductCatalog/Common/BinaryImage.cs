namespace DetailingArsenal.Domain.ProductCatalog {
    /// <summary>
    /// Base 64 encoded image that includes a filename, and data.
    /// </summary>
    public class BinaryImage : ValueObject<BinaryImage> {
        public string Name { get; }
        public byte[] Data { get; }

        public BinaryImage(string name, byte[] data) {
            Name = name;
            Data = data;
        }
    }
}
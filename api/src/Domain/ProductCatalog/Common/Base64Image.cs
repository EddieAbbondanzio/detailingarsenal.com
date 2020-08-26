namespace DetailingArsenal.Domain.ProductCatalog {
    /// <summary>
    /// Base 64 encoded image that includes a filename, and data.
    /// </summary>
    public class Base64Image : ValueObject<Base64Image> {
        /// <summary>
        /// Filename.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Base 64 encoded image data
        /// </summary>
        public string Data { get; }

        public Base64Image(string name, string data) {
            Name = name;
            Data = data;
        }

        public Base64Image(string name, byte[] data) {
            Name = name;
            Data = System.Convert.ToBase64String(data);
        }

        /// <summary>
        /// Convert the Base64 image data to it's raw byte array format.
        /// </summary>
        public byte[] ToByteArray() => System.Convert.FromBase64String(Data);
    }
}
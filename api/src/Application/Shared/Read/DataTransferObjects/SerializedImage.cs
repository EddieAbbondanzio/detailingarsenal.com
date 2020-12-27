namespace DetailingArsenal.Application.Shared {
    public class SerializedImage : IDataTransferObject {
        public byte[] Data { get; }
        public string MimeType { get; }

        public SerializedImage(byte[] data, string mimeType) {
            Data = data;
            MimeType = mimeType;
        }
    }
}
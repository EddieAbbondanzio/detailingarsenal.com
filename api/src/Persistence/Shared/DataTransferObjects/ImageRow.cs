using System;

namespace DetailingArsenal.Persistence.Shared {
    public class ImageRow : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string FileName { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public byte[] ImageData { get; set; } = null!;
        public byte[] ThumbnailData { get; set; } = null!;
    }
}
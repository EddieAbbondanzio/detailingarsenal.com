using System;
using System.Drawing;

namespace DetailingArsenal.Domain.Shared {
    public class ProcessedImage : Entity<ProcessedImage> {
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public Image Full { get; set; }
        public Image Thumbnail { get; set; }

        public ProcessedImage(Guid id, string fileName, string mimeType, Image full, Image thumbnail) {
            Id = id;
            FileName = fileName;
            MimeType = mimeType;
            Full = full;
            Thumbnail = thumbnail;
        }

        public ProcessedImage(string fileName, string mimeType, Image full, Image thumbnail) {
            Id = Guid.NewGuid();
            FileName = fileName;
            MimeType = mimeType;
            Full = full;
            Thumbnail = thumbnail;
        }
    }
}
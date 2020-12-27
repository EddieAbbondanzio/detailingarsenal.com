using System;
using System.Drawing;

namespace DetailingArsenal.Domain.Shared {
    public class ProcessedImage : Entity<ProcessedImage> {
        public ImageParentReference Parent { get; set; }
        public string FileName { get; set; }
        public Image Full { get; set; }
        public Image Thumbnail { get; set; }

        public ProcessedImage(Guid id, ImageParentReference parent, string fileName, Image full, Image thumbnail) {
            Id = id;
            Parent = parent;
            FileName = fileName;
            Full = full;
            Thumbnail = thumbnail;
        }

        public ProcessedImage(string fileName, Image full, Image thumbnail) {
            Id = Guid.NewGuid();
            Parent = null!; //TODO: Fix this hack
            FileName = fileName;
            Full = full;
            Thumbnail = thumbnail;
        }
    }
}
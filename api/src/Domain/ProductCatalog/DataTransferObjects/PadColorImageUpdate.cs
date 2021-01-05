using System;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PadColorImageUpdate {
        public Guid? ExistingId { get; set; }
        public DataUrlImage? NewImage { get; set; } = null!;

        public PadColorImageUpdateAction Action {
            get {
                if (ExistingId == null && NewImage == null) {
                    return PadColorImageUpdateAction.DeleteImage;
                } else if (ExistingId == null && NewImage != null) {
                    return PadColorImageUpdateAction.ReplaceImage;
                } else if (ExistingId != null && NewImage == null) {
                    return PadColorImageUpdateAction.DoNothing;
                } else {
                    throw new InvalidOperationException();
                }
            }
        }
    }

    public enum PadColorImageUpdateAction {
        DoNothing,
        ReplaceImage,
        DeleteImage
    }
}
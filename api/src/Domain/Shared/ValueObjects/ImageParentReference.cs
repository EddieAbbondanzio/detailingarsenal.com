using System;

namespace DetailingArsenal.Domain.Shared {
    public class ImageParentReference : ValueObject<ImageParentReference> {
        public Guid ParentId { get; }
        public ImageParentType ParentType { get; }

        public ImageParentReference(Guid parentId, ImageParentType parentType) {
            ParentId = parentId;
            ParentType = parentType;
        }
    }
}
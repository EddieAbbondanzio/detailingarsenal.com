using System;

namespace DetailingArsenal.Domain.Shared {
    public enum ImageParentType {
        PadColor
    }

    public static class ImageParentTypeUtils {
        public static ImageParentType Parse(string imageParentType) => imageParentType switch {
            "pad_color" => ImageParentType.PadColor,
            _ => throw new NotSupportedException()
        };

        public static string Serialize(this ImageParentType imageParentType) => imageParentType switch {
            ImageParentType.PadColor => "pad_color",
            _ => throw new NotSupportedException()
        };
    }
}
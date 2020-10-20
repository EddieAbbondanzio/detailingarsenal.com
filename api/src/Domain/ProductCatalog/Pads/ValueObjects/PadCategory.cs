using System;
using System.Runtime.Serialization;

namespace DetailingArsenal.Domain.ProductCatalog {
    public enum PadCategory {
        Cutting = 0,
        Polishing = 1,
        Finishing = 2
    }

    public static class PadCategoryUtils {
        public static PadCategory Parse(string category) => category switch {
            "cutting" => PadCategory.Cutting,
            "polishing" => PadCategory.Polishing,
            "finishing" => PadCategory.Finishing,
            _ => throw new NotSupportedException()
        };

        public static string Serialize(this PadCategory category) => category switch {
            PadCategory.Cutting => "cutting",
            PadCategory.Polishing => "polishing",
            PadCategory.Finishing => "finishing",
            _ => throw new NotSupportedException()
        };
    }
}
using System;
using System.Runtime.Serialization;

namespace DetailingArsenal.Domain.ProductCatalog {
    public enum PadCategory {
        HeavyCut = 0,
        MediumCut = 1,
        HeavyPolish = 2,
        MediumPolish = 3,
        SoftPolish = 4,
        Finishing = 5
    }

    public static class PadCategoryUtils {
        public static PadCategory Parse(string category) => category switch
        {
            "heavy_cut" => PadCategory.HeavyCut,
            "medium_cut" => PadCategory.MediumCut,
            "heavy_polish" => PadCategory.HeavyPolish,
            "medium_polish" => PadCategory.MediumPolish,
            "soft_polish" => PadCategory.SoftPolish,
            "finishing" => PadCategory.Finishing,
            _ => throw new NotSupportedException()
        };

        public static string Serialize(this PadCategory category) => category switch
        {
            PadCategory.HeavyCut => "heavy_cut",
            PadCategory.MediumCut => "medium_cut",
            PadCategory.HeavyPolish => "heavy_polish",
            PadCategory.MediumPolish => "medium_polish",
            PadCategory.SoftPolish => "soft_polish",
            PadCategory.Finishing => "finishing",
            _ => throw new NotSupportedException()
        };
    }
}
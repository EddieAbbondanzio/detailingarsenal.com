using System;
using System.Runtime.Serialization;

namespace DetailingArsenal.Domain.ProductCatalog {
    public enum PadCategory {
        [EnumMember(Value = "heavy_cut")]
        HeavyCut = 0,
        [EnumMember(Value = "medium_cut")]
        MediumCut = 1,
        [EnumMember(Value = "heavy_polish")]
        HeavyPolish = 2,
        [EnumMember(Value = "medium_polish")]
        MediumPolish = 3,
        [EnumMember(Value = "soft_polish")]
        SoftPolish = 4,
        [EnumMember(Value = "finishing")]
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
    }
}
using System;
using DetailingArsenal.Domain.Admin.ProductCatalog;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog {
    internal static class MeasurementUnitUtils {
        public static string Serialize(this MeasurementUnit unit) => unit switch {
            MeasurementUnit.Inches => "in",
            MeasurementUnit.Millimeters => "mm",
            _ => throw new NotSupportedException($"Invalid measurement unit of {unit}")
        };

        public static MeasurementUnit Parse(string unit) => unit switch {
            "in" => MeasurementUnit.Inches,
            "mm" => MeasurementUnit.Millimeters,
            _ => throw new NotSupportedException($"Invalid measurement unit of {unit}")
        };
    }
}
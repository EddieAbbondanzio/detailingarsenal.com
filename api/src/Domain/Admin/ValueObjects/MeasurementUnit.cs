using System;

namespace DetailingArsenal.Domain.Admin.ProductCatalog {
    public enum MeasurementUnit {
        [JsonValue("in")]
        Inches,
        [JsonValue("mm")]
        Millimeters
    }
}
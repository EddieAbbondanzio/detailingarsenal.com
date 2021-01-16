using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    public enum MeasurementUnit {
        [JsonValue("in")]
        Inches,
        [JsonValue("mm")]
        Millimeters
    }
}
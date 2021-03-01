using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Domain.ProductCatalog {
    public enum PadCategory {
        None,
        Cutting,
        Polishing,
        Finishing
    }
}
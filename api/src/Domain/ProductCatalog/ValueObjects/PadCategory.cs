using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Domain.ProductCatalog {
    [Flags]
    public enum PadCategory {
        None = 0,
        Cutting = 1,
        Polishing = 2,
        Finishing = 4
    }
}
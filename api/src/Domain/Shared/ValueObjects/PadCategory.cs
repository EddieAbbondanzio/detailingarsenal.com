using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Domain.Shared {
    public enum PadCategory {
        None,
        Cutting,
        Polishing,
        Finishing
    }
}
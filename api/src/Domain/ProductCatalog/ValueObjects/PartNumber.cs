using System;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PartNumber : Entity<PartNumber> {
        public string Value { get; }
        public string? Notes { get; }

        public PartNumber(string value, string? notes = null) {
            Id = Guid.NewGuid();
            Value = value;
            Notes = notes;
        }

        [JsonConstructor]
        public PartNumber(Guid? id, string value, string? notes = null) {
            Id = id ?? Guid.NewGuid();
            Value = value;
            Notes = notes;
        }
    }
}
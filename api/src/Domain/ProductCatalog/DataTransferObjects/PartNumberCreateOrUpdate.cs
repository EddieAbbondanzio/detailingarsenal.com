using System;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Domain.ProductCatalog {
    public class PartNumberCreateOrUpdate : IDataTransferObject {
        public Guid? Id { get; }
        public string Value { get; }
        public string? Notes { get; }

        [JsonConstructor]
        public PartNumberCreateOrUpdate(Guid? id, string value, string? notes) {
            Id = id;
            Value = value;
            Notes = notes;
        }
    }
}
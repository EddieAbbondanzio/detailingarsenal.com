namespace DetailingArsenal.Domain.ProductCatalog {
    public class PartNumber : ValueObject<PartNumber> {
        public string Value { get; }
        public string? Notes { get; }

        public PartNumber(string value, string? notes = null) {
            Value = value;
            Notes = notes;
        }
    }
}
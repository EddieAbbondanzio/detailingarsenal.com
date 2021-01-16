using System;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Domain.ProductCatalog {
    /// <summary>
    /// Distance measurement that includes units.
    /// </summary>
    public class Measurement : ValueObject<Measurement> {
        public float Amount { get; }
        public MeasurementUnit Unit { get; }

        [JsonConstructor]
        public Measurement(float amount, MeasurementUnit unit) {
            if (amount <= 0) {
                throw new ArgumentOutOfRangeException($"Amount must be greater than 0. Value was: {amount}");
            }

            Amount = amount;
            Unit = unit;
        }

        public Measurement(float amount, string unit) : this(amount, MeasurementUnitUtils.Parse(unit)) { }


        public override bool Equals(object? obj) => obj is Measurement measurement &&
            Amount == measurement.Amount &&
            Unit == measurement.Unit;

        public override int GetHashCode() => HashCode.Combine(Amount, Unit);
    }
}
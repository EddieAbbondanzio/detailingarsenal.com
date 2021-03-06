using System;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Domain.Admin.ProductCatalog {
    /// <summary>
    /// Distance measurement that includes units.
    /// </summary>
    public class Measurement : ValueObject<Measurement>, IComparable<Measurement> {
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

        public Measurement(float amount, string unit) {
            if (amount <= 0) {
                throw new ArgumentOutOfRangeException($"Amount must be greater than 0. Value was: {amount}");
            }

            Amount = amount;

            Unit = unit switch {
                "in" => MeasurementUnit.Inches,
                "mm" => MeasurementUnit.Millimeters,
                _ => throw new ArgumentOutOfRangeException("WHAT")
            };
        }

        public Measurement ToInches() {
            if (Unit == MeasurementUnit.Inches) {
                return this;
            } else {
                return new Measurement(Amount / 25.4f, MeasurementUnit.Inches);
            }
        }

        public Measurement ToMillimeters() {
            if (Unit == MeasurementUnit.Millimeters) {
                return this;
            } else {
                return new Measurement(Amount * 25.4f, MeasurementUnit.Inches);
            }
        }

        public override bool Equals(object? obj) => obj is Measurement measurement &&
            Amount == measurement.Amount &&
            Unit == measurement.Unit;

        public override int GetHashCode() => HashCode.Combine(Amount, Unit);

        public int CompareTo(Measurement? other) {
            if (other == null) {
                return 1;
            }

            var thisInches = ToInches().Amount;
            var thatInches = other.ToInches().Amount;

            if (thisInches > thatInches) {
                return 1;
            } else if (thisInches == thatInches) {
                return 0;
            } else {
                return -1;
            }

        }
    }
}
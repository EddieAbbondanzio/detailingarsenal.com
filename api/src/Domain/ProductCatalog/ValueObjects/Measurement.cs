using System;

namespace DetailingArsenal.Domain.ProductCatalog {
    /// <summary>
    /// Distance measurement that includes units.
    /// </summary>
    public class Measurement : ValueObject<Measurement> {
        public float Amount { get; }
        public MeasurementUnit Unit { get; }

        public Measurement(float amount, MeasurementUnit unit) {
            if (amount <= 0) {
                throw new ArgumentOutOfRangeException("Amount must be greater than 0");
            }

            Amount = amount;
            Unit = unit;
        }
    }
}
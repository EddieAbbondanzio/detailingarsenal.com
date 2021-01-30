namespace DetailingArsenal.Application.ProductCatalog {
    public record MeasurementReadModel : IDataTransferObject {
        /// <summary>
        /// Decimal amount
        /// </summary>
        public float Amount { get; }

        /// <summary>
        /// 2 letter abbreviation of the measurement unit.
        /// 'in', or 'mm'.
        /// </summary>
        public string Unit { get; }

        public MeasurementReadModel(float amount, string unit) {
            Amount = amount;
            Unit = unit;
        }
    }
}
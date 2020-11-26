namespace DetailingArsenal.Domain.Scheduling.Billing {
    /// <summary>
    /// Payment method expiration date.
    /// </summary>
    public class ExpirationDate : ValueObject<ExpirationDate> {
        public string Month { get; }
        public string Year { get; }

        public ExpirationDate(string month, string year) {
            Month = month;
            Year = year;
        }
    }
}
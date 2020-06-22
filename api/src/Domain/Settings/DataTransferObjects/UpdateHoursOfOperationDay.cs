namespace DetailingArsenal.Domain.Settings {
    public class UpdateHoursOfOperationDay : IDataTransferObject {
        public int Day { get; }
        public int Open { get; }
        public int Close { get; }
        public bool Enabled { get; }

        public UpdateHoursOfOperationDay(int day, int open, int close, bool enabled) {
            Day = day;
            Open = open;
            Close = close;
            Enabled = enabled;
        }
    }
}
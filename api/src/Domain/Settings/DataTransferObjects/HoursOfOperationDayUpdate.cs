namespace DetailingArsenal.Domain.Settings {
    public class HoursOfOperationDayUpdate : IDataTransferObject {
        public int Day { get; }
        public int Open { get; }
        public int Close { get; }
        public bool Enabled { get; }

        public HoursOfOperationDayUpdate(int day, int open, int close, bool enabled) {
            Day = day;
            Open = open;
            Close = close;
            Enabled = enabled;
        }
    }
}
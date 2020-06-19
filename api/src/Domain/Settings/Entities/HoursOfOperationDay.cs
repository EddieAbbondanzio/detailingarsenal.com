using System;

namespace DetailingArsenal.Domain.Settings {
    public class HoursOfOperationDay : Entity<HoursOfOperationDay> {
        public Guid HoursOfOperationId { get; set; } = Guid.Empty;
        public int Day { get; set; }
        public int Open {
            get => open;
            set {
                if (value < 0 || value > 24 * 60) {
                    throw new ArgumentOutOfRangeException();
                }

                open = value;
            }
        }
        public int Close {
            get => close;
            set {
                if (value < 0 || value > 24 * 60) {
                    throw new ArgumentOutOfRangeException();
                }

                close = value;
            }
        }
        public bool Enabled { get; set; }

        private int open;
        private int close;

        public static HoursOfOperationDay Create(Guid hoursOfOperationId, int day, int open, int close, bool enabled = true) {
            return new HoursOfOperationDay() {
                Id = Guid.NewGuid(),
                HoursOfOperationId = hoursOfOperationId,
                Day = day,
                Open = open,
                Close = close,
                Enabled = enabled
            };
        }
    }
}
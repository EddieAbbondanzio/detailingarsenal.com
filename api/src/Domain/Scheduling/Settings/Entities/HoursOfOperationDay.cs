using System;

namespace DetailingArsenal.Domain.Settings {
    public class HoursOfOperationDay : Entity<HoursOfOperationDay> {
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

        public static HoursOfOperationDay Create(int day, int open, int close, bool enabled = true) {
            return new HoursOfOperationDay() {
                Id = Guid.NewGuid(),
                Day = day,
                Open = open,
                Close = close,
                Enabled = enabled
            };
        }
    }
}
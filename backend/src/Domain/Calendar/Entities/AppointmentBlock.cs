
using System;

namespace DetailingArsenal.Domain {
    public class AppointmentBlock : Entity<AppointmentBlock> {
        public const int TimeMaxValue = 24 * 60;
        public const int DurationMaxValue = 24 * 60;

        /// <summary>
        /// Id of the appointment the time belongs to.
        /// </summary>
        public Guid AppointmentId { get; set; }

        /// <summary>
        /// Date that the appointment occurs on.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Time in minutes from midnight.
        /// </summary>
        public int Time {
            get => time;
            set {
                if (value < 0 || value > TimeMaxValue) {
                    throw new ArgumentOutOfRangeException();
                }

                time = value;
            }
        }

        /// <summary>
        /// How many minutes the appointment will take.
        /// </summary>
        public int Duration {
            get => duration;
            set {
                if (value < 0 || value > DurationMaxValue) {
                    throw new ArgumentOutOfRangeException();
                }

                duration = value;
            }
        }

        private int time;
        private int duration;
    }
}
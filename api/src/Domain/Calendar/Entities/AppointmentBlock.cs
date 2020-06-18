
using System;

namespace DetailingArsenal.Domain {
    public class AppointmentBlock : Entity<AppointmentBlock> {
        public Guid AppointmentId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Duration {
            get => Convert.ToInt32((End - Start).TotalMinutes);
        }

        public static AppointmentBlock Create(Guid appointmentId, DateTime start, DateTime end) {
            return new AppointmentBlock() {
                Id = Guid.NewGuid(),
                AppointmentId = appointmentId,
                Start = start,
                End = end
            };
        }
    }
}
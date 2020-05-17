
using System;

namespace DetailingArsenal.Domain {
    public class AppointmentBlock : Entity<AppointmentBlock> {
        public Guid AppointmentId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Duration {
            get => Convert.ToInt32((End - Start).TotalMinutes);
        }
    }
}
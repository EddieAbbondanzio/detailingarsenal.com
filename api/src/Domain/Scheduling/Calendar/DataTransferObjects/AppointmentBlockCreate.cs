using System;

namespace DetailingArsenal.Domain.Calendar {
    public class AppointmentBlockCreate : IDataTransferObject {
        public DateTime Start { get; }
        public DateTime End { get; }

        public AppointmentBlockCreate(DateTime start, DateTime end) {
            Start = start;
            End = end;
        }
    }
}
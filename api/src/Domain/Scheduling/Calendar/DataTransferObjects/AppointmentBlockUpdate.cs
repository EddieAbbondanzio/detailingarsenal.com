using System;

namespace DetailingArsenal.Domain.Calendar {
    public class AppointmentBlockUpdate : IDataTransferObject {
        public DateTime Start { get; }
        public DateTime End { get; }

        public AppointmentBlockUpdate(DateTime start, DateTime end) {
            Start = start;
            End = end;
        }
    }
}
using System;

namespace DetailingArsenal.Domain.Calendar {
    public class UpdateAppointmentBlock : IDataTransferObject {
        public DateTime Start { get; }
        public DateTime End { get; }

        public UpdateAppointmentBlock(DateTime start, DateTime end) {
            Start = start;
            End = end;
        }
    }
}
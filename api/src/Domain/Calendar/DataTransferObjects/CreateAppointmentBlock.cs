using System;

namespace DetailingArsenal.Domain.Calendar {
    public class CreateAppointmentBlock : IDataTransferObject {
        public DateTime Start { get; }
        public DateTime End { get; }

        public CreateAppointmentBlock(DateTime start, DateTime end) {
            Start = start;
            End = end;
        }
    }
}
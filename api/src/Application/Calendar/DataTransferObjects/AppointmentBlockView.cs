using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Calendar {
    public class AppointmentBlockView : IDataTransferObject {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
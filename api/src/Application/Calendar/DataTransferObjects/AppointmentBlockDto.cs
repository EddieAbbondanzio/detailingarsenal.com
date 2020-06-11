using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class AppointmentBlockDto : IDataTransferObject {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
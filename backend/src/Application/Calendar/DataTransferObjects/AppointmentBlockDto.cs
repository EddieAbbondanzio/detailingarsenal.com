using System;

namespace DetailingArsenal.Application {
    public class AppointmentBlockDto : IDataTransferObject {
        public DateTime Date { get; set; }
        public int Time { get; set; }
        public int Duration { get; set; }
    }
}
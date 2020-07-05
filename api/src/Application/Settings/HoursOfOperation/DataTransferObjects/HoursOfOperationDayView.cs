using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Settings {
    public class HoursOfOperationDayView : IDataTransferObject {
        public Guid Id { get; set; }
        public int Day { get; set; }
        public int Open { get; set; }
        public int Close { get; set; }
        public bool Enabled { get; set; }
    }
}
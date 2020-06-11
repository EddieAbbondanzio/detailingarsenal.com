using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    public class HoursOfOperationDayDto : IDataTransferObject {
        public Guid Id { get; set; }
        public int Day { get; set; }
        public int Open { get; set; }
        public int Close { get; set; }
        public bool Enabled { get; set; }
    }
}
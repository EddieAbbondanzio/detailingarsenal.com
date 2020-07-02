using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence.Settings {
    public class HoursOfOperationDayModel : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid HoursOfOperationId { get; set; }
        public int Day { get; set; }
        public int Open { get; set; }
        public int Close { get; set; }
        public bool Enabled { get; set; }
    }
}
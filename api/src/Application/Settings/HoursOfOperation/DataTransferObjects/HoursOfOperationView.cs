using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Settings {
    public class HoursOfOperationView : IDataTransferObject {
        public Guid Id { get; set; }
        public IEnumerable<HoursOfOperationDayDto> Days { get; set; } = null!;
    }
}
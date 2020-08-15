using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Settings {
    public class HoursOfOperationUpdate : IDataTransferObject {
        public List<HoursOfOperationDayUpdate> Days { get; }

        public HoursOfOperationUpdate(List<HoursOfOperationDayUpdate> days) {
            Days = days;
        }
    }
}
using System;
using System.Collections.Generic;

namespace DetailingArsenal.Domain.Settings {
    public class UpdateHoursOfOperation : IDataTransferObject {
        public List<UpdateHoursOfOperationDay> Days { get; }

        public UpdateHoursOfOperation(List<UpdateHoursOfOperationDay> days) {
            Days = days;
        }
    }
}
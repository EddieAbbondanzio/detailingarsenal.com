using System;

namespace DetailingArsenal.Application.Scheduling.Billing {
    public class PeriodReadModel : IDataTransferObject {
        public DateTime Start { get; }
        public DateTime End { get; }

        public PeriodReadModel(DateTime start, DateTime end) {
            Start = start;
            End = end;
        }
    }
}
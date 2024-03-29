using System;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    public class Period : ValueObject<Period> {
        public DateTime Start { get; }
        public DateTime End { get; }

        public Period(DateTime start, DateTime end) {
            Start = start;
            End = end;
        }
    }
}
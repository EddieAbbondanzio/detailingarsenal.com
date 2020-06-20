using System;
using System.Collections.Generic;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application.Settings {
    public class UpdateHoursOfOperationCommand : IAction {
        public Guid Id { get; set; }
        public List<HoursOfOperationDayDto> Days { get; set; } = new List<HoursOfOperationDayDto>();
    }
}
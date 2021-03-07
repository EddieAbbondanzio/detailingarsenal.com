using DetailingArsenal.Application.Settings;
using FluentValidation;

namespace DetailingArsenal.Domain.Settings {
    [DependencyInjection]
    public class UpdateHoursOfOperationValidator : FluentValidatorAdapter<UpdateHoursOfOperationCommand> {
        public UpdateHoursOfOperationValidator() {
            RuleFor(c => c.Days).Must((days) => days.Count <= 7).WithMessage("Maximum of 7 days allowed");
            RuleForEach(c => c.Days).Where((day) => day.Enabled).Must((day) => day.Open < day.Close).WithMessage("Open time must be before close time");
        }
    }
}

using DetailingArsenal.Domain;
using FluentValidation;

namespace DetailingArsenal.Application {
    public class UpdateEmployeeValidator : FluentValidatorAdapter<UpdateEmployeeCommand> {
        public UpdateEmployeeValidator() {
            RuleFor(cmd => cmd.Id).NotNull().WithMessage("Id is required.");
            RuleFor(cmd => cmd.Id).NotEmpty().WithMessage("Id is required.");

            RuleFor(cmd => cmd.Name).NotNull().WithMessage("Name is required.");
            RuleFor(cmd => cmd.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(cmd => cmd.Name).MaximumLength(Employee.NameMaxLength).WithMessage($"Name must be {Employee.NameMaxLength} characters or less.");

            When(cmd => cmd.Position != null, () => {
                RuleFor(cmd => cmd.Position).MaximumLength(Employee.PositionMaxLength).WithMessage($"Position must be {Employee.PositionMaxLength} characters or less.");
            });
        }
    }
}

using DetailingArsenal.Domain;
using FluentValidation;

namespace DetailingArsenal.Application {
    public class CreateEmployeeValidator : FluentValidatorAdapter<CreateEmployeeCommand> {
        public CreateEmployeeValidator() {
            RuleFor(cmd => cmd.Name).NotNull().WithMessage("Name is required.");
            RuleFor(cmd => cmd.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(cmd => cmd.Name).MaximumLength(Employee.NameMaxLength).WithMessage($"Name must be {Employee.NameMaxLength} characters or less.");

            When(cmd => cmd.Position != null, () => {
                RuleFor(cmd => cmd.Position).MaximumLength(Employee.PositionMaxLength).WithMessage($"Description must be {Employee.PositionMaxLength} characters or less.");
            });
        }
    }
}
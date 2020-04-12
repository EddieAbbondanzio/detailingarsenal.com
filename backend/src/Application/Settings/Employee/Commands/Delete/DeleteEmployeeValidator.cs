
using FluentValidation;

namespace DetailingArsenal.Application {
    public class DeleteEmployeeValidator : FluentValidatorAdapter<DeleteEmployeeCommand> {
        public DeleteEmployeeValidator() {
            RuleFor(cmd => cmd.Id).NotNull().WithMessage("Id is required.");
            RuleFor(cmd => cmd.Id).NotEmpty().WithMessage("Id is required.");
        }
    }
}
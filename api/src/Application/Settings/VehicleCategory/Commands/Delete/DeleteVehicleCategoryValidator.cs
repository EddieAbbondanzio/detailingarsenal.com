
using FluentValidation;

namespace DetailingArsenal.Application {
    public class DeleteVehicleCategoryValidator : FluentValidatorAdapter<DeleteVehicleCategoryCommand> {
        public DeleteVehicleCategoryValidator() {
            RuleFor(cmd => cmd.Id).NotNull().WithMessage("Id is required.");
            RuleFor(cmd => cmd.Id).NotEmpty().WithMessage("Id is required.");
        }
    }
}
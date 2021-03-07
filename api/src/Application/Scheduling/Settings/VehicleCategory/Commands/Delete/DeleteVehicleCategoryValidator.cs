
using DetailingArsenal.Domain;
using FluentValidation;

namespace DetailingArsenal.Application.Settings {
    [DependencyInjection]
    public class DeleteVehicleCategoryValidator : FluentValidatorAdapter<DeleteVehicleCategoryCommand> {
        public DeleteVehicleCategoryValidator() {
            RuleFor(cmd => cmd.Id).NotNull().WithMessage("Id is required.");
            RuleFor(cmd => cmd.Id).NotEmpty().WithMessage("Id is required.");
        }
    }
}
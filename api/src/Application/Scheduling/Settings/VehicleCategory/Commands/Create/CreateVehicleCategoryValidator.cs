
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using FluentValidation;

namespace DetailingArsenal.Application.Settings {
    [DependencyInjection]
    public class CreateVehicleCategoryValidator : FluentValidatorAdapter<CreateVehicleCategoryCommand> {
        public CreateVehicleCategoryValidator() {
            RuleFor(cmd => cmd.Name).NotNull().WithMessage("Name is required.");
            RuleFor(cmd => cmd.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(cmd => cmd.Name).MaximumLength(VehicleCategory.NameMaxLength).WithMessage($"Name must be {VehicleCategory.NameMaxLength} characters or less.");

            When(cmd => cmd.Description != null, () => {
                RuleFor(cmd => cmd.Description).MaximumLength(VehicleCategory.DescriptionMaxLength).WithMessage($"Description must be {VehicleCategory.DescriptionMaxLength} characters or less.");
            });
        }
    }
}
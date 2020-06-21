using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using FluentValidation;

namespace DetailingArsenal.Application.Settings {
    public class CreateServiceValidator : FluentValidatorAdapter<CreateServiceCommand> {
        public CreateServiceValidator() {
            RuleFor(c => c.Name).NotNull().WithMessage("Name is required.");
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(cmd => cmd.Name).MaximumLength(Service.NameMaxLength).WithMessage($"Name must be {Service.NameMaxLength} characters or less.");

            RuleFor(cmd => cmd.Description).MaximumLength(Service.DescriptionMaxLength).WithMessage($"Description must be {Service.DescriptionMaxLength} characters or less.");
        }
    }
}
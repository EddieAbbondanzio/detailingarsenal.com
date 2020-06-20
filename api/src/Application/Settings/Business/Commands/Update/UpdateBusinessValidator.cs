using DetailingArsenal.Domain;
using FluentValidation;
using DetailingArsenal.Domain.Settings;

namespace DetailingArsenal.Application.Settings {
    public class UpdateBusinessValidator : FluentValidatorAdapter<UpdateBusinessCommand> {
        public UpdateBusinessValidator() {
            RuleFor(c => c.Id).NotNull().WithMessage("Id is required.");
            RuleFor(c => c.Id).NotEmpty().WithMessage("Id is required.");

            RuleFor(c => c.Name).MaximumLength(Business.NameMaxLength).WithMessage($"Name must be {Business.NameMaxLength} characters or less.");

            RuleFor(c => c.Address).MaximumLength(Business.AddressMaxLength).WithMessage($"Address must be {Business.AddressMaxLength} characters or less.");

            RuleFor(c => c.Phone).MaximumLength(Business.PhoneMaxLength).WithMessage($"Phone must be {Business.PhoneMaxLength} characters or less.");
        }
    }
}
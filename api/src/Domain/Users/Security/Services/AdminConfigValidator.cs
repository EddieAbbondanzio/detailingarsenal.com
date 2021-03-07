using FluentValidation;

namespace DetailingArsenal.Domain.Users {
    [DependencyInjection]
    public class AdminConfigValidator : FluentValidatorAdapter<AdminConfig> {
        public AdminConfigValidator() {
            RuleFor(c => c.Email).NotEmpty().WithMessage("Admin user email is required.");
            RuleFor(c => c.Email).EmailAddress().WithMessage("Admin user email must be a valid email.");
            RuleFor(c => c.Password).NotEmpty().WithMessage("Admin password is required.");
        }
    }
}
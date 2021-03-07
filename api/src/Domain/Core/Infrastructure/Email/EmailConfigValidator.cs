using FluentValidation;

namespace DetailingArsenal.Domain {
    [DependencyInjection]
    public class EmailConfigValidator : FluentValidatorAdapter<EmailConfig> {
        public EmailConfigValidator() {
            RuleFor(c => c.Username).NotEmpty().WithMessage("Email address is required.");
            RuleFor(c => c.Username).EmailAddress().WithMessage("Email address must be valid.");
            RuleFor(c => c.Password).NotEmpty().WithMessage("Email password is required.");
            RuleFor(c => c.SMTP).NotEmpty().WithMessage("Email SMTP is required.");
            RuleFor(c => c.Port).GreaterThan(0).WithMessage("Email port is required.");
        }
    }
}
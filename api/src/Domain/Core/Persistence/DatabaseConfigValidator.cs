using FluentValidation;

namespace DetailingArsenal.Domain {
    [DependencyInjection]
    public class DatabaseConfigValidator : FluentValidatorAdapter<DatabaseConfig> {
        public DatabaseConfigValidator() {
            RuleFor(c => c.Host).NotEmpty().WithMessage("Database host is required.");
            RuleFor(c => c.Port).GreaterThan(0).WithMessage("Database port is required.");
            RuleFor(c => c.User).NotEmpty().WithMessage("Database user is required.");
            RuleFor(c => c.Password).NotEmpty().WithMessage("Database password is required.");
            RuleFor(c => c.Database).NotEmpty().WithMessage("Database name is required.");
        }
    }
}
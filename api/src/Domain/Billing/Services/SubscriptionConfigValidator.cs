using FluentValidation;

namespace DetailingArsenal.Domain.Billing {
    public class SubscriptionConfigValidator : FluentValidatorAdapter<ISubscriptionConfig> {
        public SubscriptionConfigValidator() {
            RuleFor(c => c.SecretKey).NotEmpty().WithMessage("No secret key.");

            RuleFor(c => c.DefaultPlan).NotEmpty().WithMessage("No default subscription plan.");
            RuleFor(c => c.DefaultPlan).Must((p) => p.StartsWith("prod_")).WithMessage("Invalid default plan format.");

            RuleFor(c => c.DefaultPrice).NotEmpty().WithMessage("No default price.");
            RuleFor(c => c.DefaultPrice).Must((p) => p.StartsWith("price_")).WithMessage("Invalid default price format.");

            RuleFor(c => c.TrialPeriod).GreaterThan(0).WithMessage("Trial period must be greater than 0.");
        }
    }
}
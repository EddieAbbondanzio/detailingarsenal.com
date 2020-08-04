using FluentValidation;

namespace DetailingArsenal.Domain.Billing {
    public class BillingConfigValidator : FluentValidatorAdapter<IBillingConfig> {
        public BillingConfigValidator() {
            RuleFor(c => c.SecretKey).NotEmpty().WithMessage("No secret key.");

            RuleFor(c => c.DefaultPlan).NotEmpty().WithMessage("No default subscription plan.");
            RuleFor(c => c.DefaultPlan).Must((p) => p.StartsWith("prod_")).WithMessage("Invalid default plan format.");

            RuleFor(c => c.DefaultPrice).NotEmpty().WithMessage("No default price.");
            RuleFor(c => c.DefaultPrice).Must((p) => p.StartsWith("price_")).WithMessage("Invalid default price format.");

            RuleFor(c => c.TrialPeriod).GreaterThan(0).WithMessage("Trial period must be greater than 0.");

            RuleFor(c => c.SuccessUrl).NotEmpty().WithMessage("No success url");
            RuleFor(c => c.CancelUrl).NotEmpty().WithMessage("No cancel url");

            RuleFor(c => c.WebhookSecret).NotNull().WithMessage("No webhook secret set");
        }
    }
}
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;

namespace DetailingArsenal.Infrastructure.Billing {
    public class StripeConfig : ISubscriptionConfig {
        /// <summary>
        /// Secret API key for Stripe.
        /// </summary>
        /// <value></value>
        public string SecretKey { get; set; } = null!;

        /// <summary>
        /// Id of the plan to use when a new user signs up.
        /// </summary>
        public string DefaultPlan { get; set; } = null!;

        /// <summary>
        /// Id of the price to use for the default plan.
        /// </summary>
        public string DefaultPrice { get; set; } = null!;
        public int TrialPeriod { get; set; }
    }
}
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;

namespace DetailingArsenal.Infrastructure.Scheduling.Billing {
    public class StripeConfig : IBillingConfig {
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

        /// <summary>
        /// How many days the trial should run for.
        /// </summary>
        public int TrialPeriod { get; set; }

        /// <summary>
        /// Used by checkout to redirect after a successful subscription start.
        /// </summary>
        public string SuccessUrl { get; set; } = null!;

        /// <summary>
        /// Used by checkout to redirect after a non-successful subscription start.
        /// </summary>
        /// <value></value>
        public string CancelUrl { get; set; } = null!;

        /// <summary>
        /// Secret used to sign webhooks to verify they came from a legit sender.
        /// </summary>
        public string WebhookSecret { get; set; } = null!;
    }
}
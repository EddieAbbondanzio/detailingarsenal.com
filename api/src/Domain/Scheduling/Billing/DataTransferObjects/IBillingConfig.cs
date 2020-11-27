namespace DetailingArsenal.Domain.Scheduling.Billing {
    public interface IBillingConfig {
        string SecretKey { get; set; }
        /// <summary>
        /// Billing ID of the default plan. (BillingReferenceType.Product)
        /// </summary>
        /// <value></value>
        string DefaultPlan { get; set; }
        string DefaultPrice { get; set; }
        int TrialPeriod { get; set; }
        string SuccessUrl { get; set; }
        string CancelUrl { get; set; }
        string WebhookSecret { get; set; }
    }
}
namespace DetailingArsenal.Domain.Billing {
    public interface ISubscriptionConfig {
        string SecretKey { get; set; }
        string DefaultPlan { get; set; }
        string DefaultPrice { get; set; }
        int TrialPeriod { get; set; }
        string SuccessUrl { get; set; }
        string CancelUrl { get; set; }
        string WebhookSecret {get;set;}
    }
}
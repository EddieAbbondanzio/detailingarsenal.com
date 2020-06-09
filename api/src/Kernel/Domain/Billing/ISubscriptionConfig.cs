public interface ISubscriptionConfig {
    string SecretKey { get; set; }
    string DefaultPlan { get; set; }
    string DefaultPrice { get; set; }
    int TrialPeriod { get; set; }
}
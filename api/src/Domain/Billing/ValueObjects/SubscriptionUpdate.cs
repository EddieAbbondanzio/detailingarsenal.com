
namespace DetailingArsenal.Domain {
    public class SubscriptionUpdate : ValueObject<SubscriptionUpdate> {
        SubscriptionUpdateType Type { get; }
        Customer Customer { get; }
        Subscription Subscription { get; }
        SubscriptionPlan Plan { get; }

        public SubscriptionUpdate(SubscriptionUpdateType type, Customer customer, Subscription subscription, SubscriptionPlan plan) {
            Type = type;
            Customer = customer;
            Subscription = subscription;
            Plan = plan;
        }
    }
}
namespace DetailingArsenal.Domain.Billing {
    public enum BillingReferenceType {
        Product = 0,
        Price = 1,
        Customer = 2,
        Subscription = 3,
        CheckoutSession = 4,
        PaymentMethod = 5
    }
}
using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence.Billing {
    public class SubscriptionModel : IDataTransferObject {
        public Guid CustomerId { get; set; }
        public string Status { get; set; } = null!;
        public BillingReferenceModel BillingReference { get; set; } = null!;
    }
}
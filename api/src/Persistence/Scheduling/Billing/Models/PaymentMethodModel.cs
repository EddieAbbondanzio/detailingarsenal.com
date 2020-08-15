using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence.Billing {
    public class PaymentMethodModel : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Brand { get; set; } = null!;
        public string Last4 { get; set; } = null!;
        public bool IsDefault { get; set; }
        public string ExpirationMonth { get; set; } = null!;
        public string ExpirationYear { get; set; } = null!;
        public Guid BillingReferenceId { get; set; }
    }
}
using System;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;

namespace DetailingArsenal.Persistence.Billing {
    public class BillingReferenceModel : IDataTransferObject {
        public Guid Id { get; set; }
        public string BillingId { get; set; } = null!;
        public BillingReferenceType Type { get; set; }
    }
}
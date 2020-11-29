using System;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;

namespace DetailingArsenal.Persistence.Scheduling.Billing {
    public class BillingReferenceRow : IDataTransferObject {
        public Guid Id { get; set; }
        public string BillingId { get; set; } = null!;
        public BillingReferenceType Type { get; set; }
    }
}
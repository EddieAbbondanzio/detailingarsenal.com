using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence.Scheduling.Billing {
    public class CustomerRow : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BillingReferenceId { get; set; }
    }
}
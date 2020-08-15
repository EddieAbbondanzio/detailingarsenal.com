using System;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence.Billing {
    public class CustomerModel : IDataTransferObject {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BillingReferenceId { get; set; }
    }
}
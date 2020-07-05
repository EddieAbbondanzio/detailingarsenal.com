using System;

namespace DetailingArsenal.Application.Billing {
    public class CreateSessionCommand : IAction {
        public string PriceBillingId { get; set; } = null!;
    }
}
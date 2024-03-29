using System;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    public class CreateCheckoutSessionCommand : IAction {
        /// <summary>
        /// External ID of the price to create the checkout session for. 
        /// (No internal ID exists due to prices being value objects).
        /// </summary>
        public string PriceBillingId { get; set; } = null!;
    }
}
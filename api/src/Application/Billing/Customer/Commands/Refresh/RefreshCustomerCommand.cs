using System;

namespace DetailingArsenal.Application.Billing {
    public class RefreshCustomerCommand : IAction {
        /// <summary>
        /// External billing id of the customer to refresh
        /// </summary>
        /// <value></value>
        public string BillingId { get; set; } = null!;
    }
}
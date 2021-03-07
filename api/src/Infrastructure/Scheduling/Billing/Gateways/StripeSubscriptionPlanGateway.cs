using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Scheduling.Billing;
using Stripe;
using Stripe.Checkout;

namespace DetailingArsenal.Infrastructure.Scheduling.Billing {
    [DependencyInjection(RegisterAs = typeof(ISubscriptionPlanGateway))]
    public class StripeSubscriptionPlanGateway : ISubscriptionPlanGateway {
        ProductService productService;
        PriceService priceService;

        public StripeSubscriptionPlanGateway() {
            this.productService = new ProductService();
            this.priceService = new PriceService();
        }

        public async Task<List<SubscriptionPlan>> GetAll() {
            var products = await productService.ListAsync();
            var plans = new List<SubscriptionPlan>();

            for (int i = 0; i < products.Count(); i++) {
                var product = products.ElementAt(i);

                // Check to see if we've given it an ID yet. 
                if (!product.Metadata.ContainsKey("Id")) {
                    var updateOpts = new ProductUpdateOptions();
                    updateOpts.Metadata = new Dictionary<string, string>();
                    updateOpts.Metadata["Id"] = Guid.NewGuid().ToString();

                    product = await productService.UpdateAsync(product.Id, updateOpts);
                }

                var plan = new SubscriptionPlan(
                    Guid.Parse(product.Metadata["Id"]),
                    product.Name,
                    product.Description,
                    BillingReference.Product(product.Id),
                    await GetPrices(product.Id)
                );

                plans.Add(plan);
            }

            return plans;
        }

        async Task<List<SubscriptionPlanPrice>> GetPrices(string planBillingId) {
            var prices = await priceService.ListAsync(new PriceListOptions() { Product = planBillingId });

            return prices.Select(p => new SubscriptionPlanPrice(
                         p.UnitAmountDecimal ?? throw new InvalidOperationException($"No price amount specified for price {p.Id}"),
                         p.Recurring.Interval,
                         BillingReference.Price(p.Id)
                         )
                    ).ToList();
        }
    }
}
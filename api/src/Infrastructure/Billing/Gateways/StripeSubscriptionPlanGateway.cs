using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;
using Stripe;

namespace DetailingArsenal.Infrastructure.Billing {
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
                if (!product.Metadata.ContainsKey("id")) {
                    var updateOpts = new ProductUpdateOptions();
                    updateOpts.Metadata = new Dictionary<string, string>();
                    updateOpts.Metadata["id"] = Guid.NewGuid().ToString();

                    product = await productService.UpdateAsync(product.Id, updateOpts);
                }

                var plan = new SubscriptionPlan() {
                    Id = Guid.Parse(product.Metadata["id"]),
                    Name = product.Name,
                    BillingReference = new BillingReference(product.Id, BillingReferenceType.Product),
                    Prices = await GetPrices(product.Id)
                };
                plans.Add(plan);
            }

            return plans;
        }

        async Task<List<SubscriptionPlanPrice>> GetPrices(string planBillingId) {
            var prices = await priceService.ListAsync(new PriceListOptions() { Product = planBillingId });

            return prices.Select(p => new SubscriptionPlanPrice(
                         p.UnitAmountDecimal ?? throw new InvalidOperationException($"No price amount specified for price {p.Id}"),
                         p.Recurring.Interval,
                         new BillingReference(p.Id, BillingReferenceType.Price)
                         )
                    ).ToList();
        }
    }
}
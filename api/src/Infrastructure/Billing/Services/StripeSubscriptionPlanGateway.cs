using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using Stripe;

namespace DetailingArsenal.Infrastructure {
    public class StripeSubscriptionPlanGateway : IExternalSubscriptionPlanGateway {
        private Stripe.ProductService productService;
        private Stripe.PriceService priceService;

        public StripeSubscriptionPlanGateway() {
            this.productService = new ProductService();
            this.priceService = new PriceService();
        }

        public async Task<List<ExternalSubscriptionPlan>> GetAll() {
            var products = await productService.ListAsync();
            var plans = new List<ExternalSubscriptionPlan>();

            foreach (Product product in products) {
                var prices = await priceService.ListAsync(new PriceListOptions() { Product = product.Id });

                plans.Add(new ExternalSubscriptionPlan(
                    product.Name,
                    product.Id,
                    prices.Select(p => new ExternalSubscriptionPlanPrice(
                        p.UnitAmountDecimal ?? throw new InvalidOperationException($"No price for product {product.Name}"),
                        p.Recurring.Interval,
                        p.Id
                        )
                    ).ToList())
                );
            }

            return plans;
        }
    }
}
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

            foreach (Product product in products) {
                var prices = await priceService.ListAsync(new PriceListOptions() { Product = product.Id });

                plans.Add(SubscriptionPlan.Create(
                    product.Name,
                    new BillingReference(product.Id, BillingReferenceType.Product),
                    null,
                    prices.Select(p => new SubscriptionPlanPrice(
                        p.UnitAmountDecimal ?? throw new InvalidOperationException($"No price for product {product.Name}"),
                        p.Recurring.Interval,
                        new BillingReference(p.Id, BillingReferenceType.Price)
                        )
                    ).ToList())
                );
            }

            return plans;
        }
    }
}
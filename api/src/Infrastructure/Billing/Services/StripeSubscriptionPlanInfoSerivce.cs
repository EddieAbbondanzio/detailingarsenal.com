using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using Stripe;

namespace DetailingArsenal.Infrastructure {
    public class StripeSubscriptionPlanInfoService : ISubscriptionPlanInfoService {
        private Stripe.ProductService productService;
        private Stripe.PriceService priceService;

        public StripeSubscriptionPlanInfoService() {
            this.productService = new ProductService();
            this.priceService = new PriceService();
        }

        public async Task<List<SubscriptionPlanInfo>> GetAll() {
            var products = await productService.ListAsync();
            var plans = new List<SubscriptionPlanInfo>();

            foreach (Product product in products) {
                var prices = await priceService.ListAsync(new PriceListOptions() { Product = product.Id });

                plans.Add(new SubscriptionPlanInfo(
                    product.Name,
                    product.Id,
                    prices.Select(p => new SubscriptionPlanPriceInfo(
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
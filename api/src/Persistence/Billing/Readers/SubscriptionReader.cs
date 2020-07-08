using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Persistence.Billing {
    public class SubscriptionReader : DatabaseInteractor, ISubscriptionReader {
        public SubscriptionReader(IDatabase database) : base(database) { }

        public async Task<SubscriptionReadModel> ReadForUser(User user) {
            var sub = await Connection.QueryFirstAsync(
                @"select sp.name as name, s.status as status, spp.price as price, spp.interval as price_interval, br.billing_id as price_billing_id from subscriptions s
                left join subscription_plans sp on s.plan_id = sp.id
                left join subscription_plan_prices spp on sp.id = spp.plan_id
                left join billing_references br on spp.billing_reference_id = br.id
                left join customers c on s.customer_id = c.id
                where c.user_id = @Id and br.billing_id = s.price_billing_id;",
                user
            );

            return new SubscriptionReadModel(
                sub.name,
                sub.status,
                new SubscriptionPlanPriceReadModel(
                    sub.price,
                    sub.price_interval,
                    sub.price_billing_id
                )
            );
        }
    }
}
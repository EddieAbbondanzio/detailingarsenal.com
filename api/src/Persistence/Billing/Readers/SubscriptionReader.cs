using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Persistence.Billing {
    public class SubscriptionReader : DatabaseInteractor, ISubscriptionReader {
        public SubscriptionReader(IDatabase database) : base(database) { }

        public async Task<SubscriptionReadModel> ReadForUser(User user) {
            var sub = await Connection.QueryFirstAsync(
                @"select sp.name name, s.status status, spp.price price, spp.interval priceInterval from subcription s
                left join subscription_plan sp on s.plan_id = sp.id
                left join subscription_plan_prices spp on sp.id = spp.plan_id
                left join customers c on s.customer_id = c.id
                where c.user_id = @Id and spp.id = s.price_billing_id;",
                user
            );

            return new SubscriptionReadModel(
                sub.name,
                sub.status,
                sub.price,
                sub.priceInterval
            );
        }
    }
}
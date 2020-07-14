using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Persistence.Billing {
    public class CustomerReader : DatabaseInteractor, ICustomerReader {
        public CustomerReader(IDatabase database) : base(database) { }

        public async Task<CustomerReadModel> ReadForUser(User user) {
            var sub = await Connection.QueryFirstAsync(
                @"select sp.name as name, s.status as status, s.next_payment as next_payment, s.trial_start as trial_start, s.trial_end as trial_end, spp.price as price, spp.interval as price_interval, br.billing_id as price_billing_id from subscriptions s
                left join subscription_plans sp on s.plan_id = sp.id
                left join subscription_plan_prices spp on sp.id = spp.plan_id
                left join billing_references br on spp.billing_reference_id = br.id
                left join customers c on s.customer_id = c.id
                where c.user_id = @Id and br.billing_id = s.price_billing_id;",
                user
            );

            return new CustomerReadModel(
                new SubscriptionReadModel(
                    sub.name,
                    new SubscriptionPlanPriceReadModel(
                        sub.price,
                        sub.price_interval,
                        sub.price_billing_id
                    ),
                    sub.status,
                    sub.next_payment,
                    sub.trial_start,
                    sub.trial_end
                )
            );
        }
    }
}
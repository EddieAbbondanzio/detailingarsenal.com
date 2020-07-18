using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Persistence.Billing {
    public class CustomerReader : DatabaseInteractor, ICustomerReader {
        public CustomerReader(IDatabase database) : base(database) { }

        public async Task<CustomerReadModel> ReadForUser(User user) {
            using (var reader = await Connection.QueryMultipleAsync(
                @"select sp.name as name, s.status as status, s.next_payment as next_payment, s.trial_start as trial_start, s.trial_end as trial_end, spp.price as price, spp.interval as price_interval, br.billing_id as price_billing_id from subscriptions s
                left join subscription_plans sp on s.plan_id = sp.id
                left join subscription_plan_prices spp on sp.id = spp.plan_id
                left join billing_references br on spp.billing_reference_id = br.id
                left join customers c on s.customer_id = c.id
                where c.user_id = @Id and br.billing_id = s.price_billing_id;
                
                select brand, last_4 from payment_methods pm
                left join customers c on c.id = pm.customer_id
                where c.user_id = @Id;
                ",
                user
            )) {

                var rawCustomer = reader.ReadFirst();

                var rawPaymentMethod = reader.ReadFirstOrDefault();
                PaymentMethodReadModel? paymentMethod = null;

                if (rawPaymentMethod != null) {
                    paymentMethod = new PaymentMethodReadModel(
                        rawPaymentMethod.brand,
                        rawPaymentMethod.last_4
                    );
                }

                return new CustomerReadModel(
                    new SubscriptionReadModel(
                        rawCustomer.name,
                        new SubscriptionPlanPriceReadModel(
                            rawCustomer.price,
                            rawCustomer.price_interval,
                            rawCustomer.price_billing_id
                        ),
                        rawCustomer.status,
                        rawCustomer.next_payment,
                        rawCustomer.trial_start,
                        rawCustomer.trial_end
                    ),
                    paymentMethod
                );
            }
        }
    }
}
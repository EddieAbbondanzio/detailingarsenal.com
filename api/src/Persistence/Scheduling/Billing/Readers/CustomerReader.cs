using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Application.Scheduling.Billing;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Persistence.Scheduling.Billing {
    public class CustomerReader : DatabaseInteractor, ICustomerReader {
        public CustomerReader(IDatabase database) : base(database) { }

        public async Task<CustomerReadModel> ReadForUser(User user) {
            using (var conn = OpenConnection()) {
                using (var reader = await conn.QueryMultipleAsync(
                    @"
                select sp.name as name, 
                s.status as status, 
                s.trial_start as trial_start, 
                s.trial_end as trial_end, 
                s.period_start,
                s.period_end,
                s.cancelling_at_period_end,
                spp.price as price, 
                spp.interval as price_interval, 
                br.billing_id as price_billing_id
                from customers c
                left join subscriptions s on c.id = s.customer_id 
                left join subscription_plans sp on s.plan_id = sp.id
                left join subscription_plan_prices spp on sp.id = spp.plan_id
                left join billing_references br on spp.billing_reference_id = br.id
                where c.user_id = @Id and br.billing_id = s.price_billing_id;
                
                select pm.* from payment_methods pm
                join customers c on c.id = pm.customer_id
                where c.user_id = @Id;
                ",
                    user
                )) {

                    // Attempt to parse in susbcription
                    var rawSubscription = reader.ReadFirstOrDefault();
                    SubscriptionReadModel? subscription = null;

                    if (rawSubscription != null) {
                        subscription = new SubscriptionReadModel(
                            rawSubscription.name,
                            new SubscriptionPlanPriceReadModel(
                                rawSubscription.price,
                                rawSubscription.price_interval,
                                rawSubscription.price_billing_id
                            ),
                            rawSubscription.status,
                            new PeriodReadModel(
                                rawSubscription.trial_start,
                                rawSubscription.trial_end
                            ),
                            new PeriodReadModel(
                                rawSubscription.period_start,
                                rawSubscription.period_end
                            ),
                            rawSubscription.cancelling_at_period_end
                        );
                    }

                    // Attempt to parse in payment methods
                    var rawPaymentMethods = reader.Read();
                    List<PaymentMethodReadModel> paymentMethods = new List<PaymentMethodReadModel>();

                    foreach (var raw in rawPaymentMethods) {
                        paymentMethods.Add(new PaymentMethodReadModel(
                            raw.brand,
                            raw.last_4,
                            raw.is_default,
                            raw.expiration_month,
                            raw.expiration_year
                        ));
                    }

                    return new CustomerReadModel(
                        subscription,
                        paymentMethods
                    );
                }
            }
        }
    }
}
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Persistence.Billing {
    public class CustomerReader : DatabaseInteractor, ICustomerReader {
        public CustomerReader(IDatabase database) : base(database) { }

        public async Task<CustomerReadModel> ReadForUser(User user) {
            using (var reader = await Connection.QueryMultipleAsync(
                @"
                select sp.name as name, 
                s.status as status, 
                s.next_payment as 
                next_payment, 
                s.trial_start as trial_start, 
                s.trial_end as trial_end, 
                spp.price as price, 
                spp.interval as price_interval, 
                br.billing_id as price_billing_id
                from customers c
                left join subscriptions s on c.id = s.customer_id 
                left join subscription_plans sp on s.plan_id = sp.id
                left join subscription_plan_prices spp on sp.id = spp.plan_id
                left join billing_references br on spp.billing_reference_id = br.id
                where c.user_id = @Id and br.billing_id = s.price_billing_id;
                
                select brand, last_4 from payment_methods pm
                left join customers c on c.id = pm.customer_id
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
                        rawSubscription.next_payment,
                        rawSubscription.trial_start,
                        rawSubscription.trial_end
                    );
                }

                // Attempt to parse in payment method
                var rawPaymentMethod = reader.ReadFirstOrDefault();
                PaymentMethodReadModel? paymentMethod = null;

                if (rawPaymentMethod != null) {
                    paymentMethod = new PaymentMethodReadModel(
                        rawPaymentMethod.brand,
                        rawPaymentMethod.last_4
                    );
                }

                return new CustomerReadModel(
                    subscription,
                    paymentMethod
                );
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;
using Serilog;

namespace DetailingArsenal.Persistence.Billing {
    public class CustomerRepo : DatabaseInteractor, ICustomerRepo {
        public CustomerRepo(IDatabase database) : base(database) { }

        public async Task<Customer?> FindById(Guid id) {
            using (var reader = await Connection.QueryMultipleAsync(
                @"select c.*, br.* from customers c
                    join billing_references br on c.billing_reference_id = br.id
                    where c.id = @Id;
                
                select s.*, br.* from subscriptions s
                    join billing_references br on s.billing_reference_id = br.id
                    join customers c on s.customer_id = c.id
                    where c.id = @Id;
                  
                select pm.*, br.* from payment_methods pm
                    join customers c on pm.customer_id = c.id
                    join billing_references br on pm.billing_reference_id = br.id
                    where c.id = @Id;
                  ",
                  new {
                      Id = id
                  }
            )) {
                return Read(reader);
            }
        }

        public async Task<Customer?> FindByBillingId(string billingId) {
            using (var reader = await Connection.QueryMultipleAsync(
                @"select c.*, br.* from customers c
                    join billing_references br on c.billing_reference_id = br.id
                    where br.billing_id = @BillingId;
                
                select s.*, sbr.* from subscriptions s
                    join billing_references sbr on s.billing_reference_id = sbr.id
                    join customers c on s.customer_id = c.id
                    join billing_references cbr on c.billing_reference_id = cbr.id
                    where cbr.billing_id = @BillingId;
                  
                select pm.*, br.* from payment_methods pm
                    join customers c on pm.customer_id = c.id
                    join billing_references br on pm.billing_reference_id = br.id
                    where br.billing_id = @BillingId;
                  ",
                  new {
                      BillingId = billingId
                  }
            )) {
                return Read(reader);
            }
        }

        public async Task<Customer?> FindByUser(User user) {
            using (var reader = await Connection.QueryMultipleAsync(
                @"select c.*, br.* from customers c
                    join billing_references br on c.billing_reference_id = br.id
                    where c.user_id = @Id;
                  
                select s.*, br.* from subscriptions s
                    join billing_references br on s.billing_reference_id = br.id
                    join customers c on s.customer_id = c.id
                    where c.user_id = @Id;

                select pm.*, br.* from payment_methods pm
                    join customers c on pm.customer_id = c.id
                    join billing_references br on pm.billing_reference_id = br.id
                    where c.user_id = @Id; 
                  ",
                user
            )) {
                return Read(reader);
            }
        }

        public async Task Add(Customer customer) {
            using (var t = Connection.BeginTransaction()) {
                // Insert customer parent record
                var customerBillingReference = new BillingReferenceModel() {
                    Id = Guid.NewGuid(),
                    BillingId = customer.BillingReference.BillingId,
                    Type = BillingReferenceType.Customer
                };

                await Connection.ExecuteAsync(
                    @"insert into billing_references (id, billing_id, type) values (@Id, @BillingId, @Type);",
                    customerBillingReference
                );

                await Connection.ExecuteAsync(
                    @"insert into customers (id, user_id, billing_reference_id) values (@Id, @UserId, @BillingReferenceId);",
                    new CustomerModel() {
                        Id = customer.Id,
                        UserId = customer.UserId,
                        BillingReferenceId = customerBillingReference.Id
                    }
                );

                await InsertPaymentMethods(customer.Id, customer.PaymentMethods);
                await InsertSubscription(customer.Id, customer.Subscription);

                t.Commit();
            }
        }

        public async Task Update(Customer customer) {
            using (var t = Connection.BeginTransaction()) {
                /*
                * No columns on the customer record to update.
                */

                await InsertPaymentMethods(customer.Id, customer.PaymentMethods, true);
                await InsertSubscription(customer.Id, customer.Subscription, true);

                t.Commit();
            }
        }

        public async Task Delete(Customer customer) {
            using (var t = Connection.BeginTransaction()) {
                // Delete subscription
                var oldSubscription = await Connection.QueryFirstOrDefaultAsync<SubscriptionModel>("select * from subscriptions where customer_id = @Id;", customer);
                await Connection.ExecuteAsync(@"delete from subscriptions where id = @Id;", oldSubscription);
                await Connection.ExecuteAsync(@"delete from billing_references where id = @BillingReferenceId;", oldSubscription);

                // Delete any payment methods
                var oldPaymentMethods = await Connection.QueryAsync<PaymentMethodModel>("select * from payment_methods where customer_id = @Id;", customer);
                await Connection.ExecuteAsync("delete from payment_methods where customer_id = @Id", customer);
                await Connection.ExecuteAsync("delete from billing_references where id = @BillingReferenceId;", oldPaymentMethods.ToList());

                // Delete customer (parent) last
                await Connection.ExecuteAsync(@"delete from customers where id = @Id", customer);
                await Connection.ExecuteAsync(@"delete from billing_references where billing_id = @BillingId", customer.BillingReference);

                t.Commit();
            }
        }

        Customer? Read(SqlMapper.GridReader reader) {
            var customer = reader.Read<CustomerModel, BillingReferenceModel, Customer>(
                (c, br) => new Customer(
                    c.Id,
                    c.UserId,
                    BillingReference.Customer(br.BillingId)
                )
            ).FirstOrDefault();

            if (customer == null) {
                return null;
            }

            customer.Subscription = reader.Read<SubscriptionModel, BillingReferenceModel, Subscription>(
                (s, br) => new Subscription(
                    s.Id,
                    s.Status,
                    new Period(
                        s.TrialStart,
                        s.TrialEnd
                    ),
                    new Period(
                        s.PeriodStart,
                        s.PeriodEnd
                    ),
                    s.CancellingAtPeriodEnd,
                    new SubscriptionPlanReference(s.PlanId, s.PriceBillingId),
                    BillingReference.Subscription(br.BillingId)
                )
            ).FirstOrDefault();

            customer.PaymentMethods = reader.Read<PaymentMethodModel, BillingReferenceModel, PaymentMethod>(
                (pm, br) => new PaymentMethod(
                    pm.Id,
                    pm.Brand,
                    pm.Last4,
                    pm.IsDefault,
                    new ExpirationDate(pm.ExpirationMonth, pm.ExpirationYear),
                    BillingReference.PaymentMethod(br.BillingId)
                )
            ).ToList();

            return customer;
        }

        async Task InsertPaymentMethods(Guid customerId, List<PaymentMethod> paymentMethods, bool deleteExisting = false) {
            if (deleteExisting) {
                var oldPaymentMethods = await Connection.QueryAsync<PaymentMethodModel>("select * from payment_methods where customer_id = @Id;", new { Id = customerId });

                await Connection.ExecuteAsync("delete from payment_methods where customer_id = @Id", new { Id = customerId });
                await Connection.ExecuteAsync("delete from billing_references where id = @BillingReferenceId;", oldPaymentMethods.ToList());
            }

            if (paymentMethods.Count > 0) {
                var pmbrs = new List<BillingReferenceModel>();
                var pms = new List<PaymentMethodModel>();

                foreach (PaymentMethod pm in paymentMethods) {
                    var brModel = new BillingReferenceModel() {
                        Id = Guid.NewGuid(),
                        BillingId = pm.BillingReference.BillingId,
                        Type = BillingReferenceType.PaymentMethod
                    };

                    var pModel = new PaymentMethodModel() {
                        Id = pm.Id,
                        BillingReferenceId = brModel.Id,
                        CustomerId = customerId,
                        Brand = pm.Brand,
                        Last4 = pm.Last4,
                        ExpirationMonth = pm.Expiration.Month,
                        ExpirationYear = pm.Expiration.Year,
                        IsDefault = pm.IsDefault
                    };

                    pmbrs.Add(brModel);
                    pms.Add(pModel);
                }

                await Connection.ExecuteAsync(
                    @"insert into billing_references (id, billing_id, type) values (@Id, @BillingId, @Type);",
                    pmbrs
                );

                await Connection.ExecuteAsync(
                    @"insert into payment_methods (
                            id, 
                            customer_id, 
                            brand, 
                            last_4, 
                            expiration_month,
                            expiration_year,
                            billing_reference_id,
                            is_default
                        ) values (
                            @Id, 
                            @CustomerId, 
                            @Brand, 
                            @Last4,
                            @ExpirationMonth, 
                            @ExpirationYear, 
                            @BillingReferenceId,
                            @IsDefault
                        );",
                    pms
                );
            }
        }

        async Task InsertSubscription(Guid customerId, Subscription? subscription, bool deleteExisting = false) {
            if (deleteExisting) {
                var oldSubscription = await Connection.QueryFirstOrDefaultAsync<SubscriptionModel>("select * from subscriptions where customer_id = @Id;", new { Id = customerId });
                await Connection.ExecuteAsync(@"delete from subscriptions where id = @Id;", oldSubscription);
                await Connection.ExecuteAsync(@"delete from billing_references where id = @BillingReferenceId;", oldSubscription);
            }

            // Insert subscription (if applicable)
            if (subscription != null) {
                var subBillingReference = new BillingReferenceModel() {
                    Id = Guid.NewGuid(),
                    BillingId = subscription.BillingReference.BillingId,
                    Type = BillingReferenceType.Subscription
                };

                await Connection.ExecuteAsync(
                    @"insert into billing_references (id, billing_id, type) values (@Id, @BillingId, @Type);",
                    subBillingReference
                );

                await Connection.ExecuteAsync(
                    @"insert into subscriptions (
                            id, 
                            plan_id, 
                            price_billing_id, 
                            customer_id, 
                            billing_reference_id, 
                            status, 
                            trial_start, 
                            trial_end, 
                            period_start,
                            period_end,
                            cancelling_at_period_end
                        ) 
                        values (
                            @Id, 
                            @PlanId, 
                            @PriceBillingId, 
                            @CustomerId, 
                            @BillingReferenceId, 
                            @Status, 
                            @TrialStart, 
                            @TrialEnd, 
                            @PeriodStart,
                            @PeriodEnd,
                            @CancellingAtPeriodEnd
                        );",
                    new SubscriptionModel() {
                        Id = subscription.Id,
                        PlanId = subscription.PlanReference.PlanId,
                        PriceBillingId = subscription.PlanReference.PriceBillingId,
                        CustomerId = customerId,
                        BillingReferenceId = subBillingReference.Id,
                        Status = subscription.Status,
                        PeriodStart = subscription.Period.Start,
                        PeriodEnd = subscription.Period.End,
                        TrialStart = subscription.TrialPeriod.Start,
                        TrialEnd = subscription.TrialPeriod.End,
                        CancellingAtPeriodEnd = subscription.CancellingAtPeriodEnd
                    }
                );
            }
        }
    }
}
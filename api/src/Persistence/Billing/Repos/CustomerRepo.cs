using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

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
                @"select s.*, sbr.* from subscriptions s
                    join billing_references sbr on s.billing_reference_id = sbr.id
                    join customers c on s.customer_id = c.id
                    join billing_references cbr on c.billing_reference_id = cbr.id
                    where cbr.billing_id = @BillingId;
                  
                select c.*, br.* from customers c
                    join billing_references br on c.billing_reference_id = br.id
                    where br.billing_id = @BillingId;
                  
                select pm.* from payment_methods pm
                    join customers c on pm.customer_id = c.id
                    join billing_references br on c.billing_reference_id = br.id
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
                @"select s.*, br.* from subscriptions s
                    join billing_references br on s.billing_reference_id = br.id
                    join customers c on s.customer_id = c.id
                    where c.user_id = @Id;
                  
                select c.*, br.* from customers c
                    join billing_references br on c.billing_reference_id = br.id
                    where c.user_id = @Id;
                  
                select pm.* from payment_methods pm
                    join customers c on pm.customer_id = c.id
                    where c.user_id = @Id; 
                  ",
                user
            )) {
                return Read(reader);
            }
        }

        public async Task Add(Customer entity) {
            using (var t = Connection.BeginTransaction()) {
                // Insert customer parent record
                var customerBillingReference = new BillingReferenceModel() {
                    Id = Guid.NewGuid(),
                    BillingId = entity.BillingReference.BillingId,
                    Type = BillingReferenceType.Customer
                };

                await Connection.ExecuteAsync(
                    @"insert into billing_references (id, billing_id, type) values (@Id, @BillingId, @Type);",
                    customerBillingReference
                );

                await Connection.ExecuteAsync(
                    @"insert into customers (id, user_id, billing_reference_id) values (@Id, @UserId, @BillingReferenceId);",
                    new CustomerModel() {
                        Id = entity.Id,
                        UserId = entity.UserId,
                        BillingReferenceId = customerBillingReference.Id
                    }
                );

                // Insert payment method (if applicable)
                if (entity.PaymentMethods.Count > 0) {
                    var paymentMethodBillingReferences = new List<BillingReferenceModel>();
                    var paymentMethods = new List<PaymentMethodModel>();

                    foreach (PaymentMethod pm in entity.PaymentMethods) {
                        var brModel = new BillingReferenceModel() {
                            Id = Guid.NewGuid(),
                            BillingId = pm.BillingReference.BillingId,
                            Type = BillingReferenceType.PaymentMethod
                        };

                        var pModel = new PaymentMethodModel() {
                            Id = pm.Id,
                            BillingReferenceId = brModel.Id,
                            CustomerId = entity.Id,
                            Brand = pm.Brand,
                            Last4 = pm.Last4,
                            IsDefault = pm.IsDefault
                        };

                        paymentMethodBillingReferences.Add(brModel);
                        paymentMethods.Add(pModel);
                    }

                    await Connection.ExecuteAsync(
                        @"insert into billing_references (id, billing_id, type) values (@Id, @BillingId, @Type);",
                        paymentMethodBillingReferences
                    );

                    await Connection.ExecuteAsync(
                        @"insert into payment_methods (
                            id, 
                            customer_id, 
                            brand, 
                            last_4, 
                            billing_reference_id
                        ) values (
                            @Id, 
                            @CustomerId, 
                            @Brand, 
                            @Last4, 
                            @BillingReferenceId
                        );",
                        paymentMethods
                    );
                }

                // Insert subscription (if applicable)
                if (entity.Subscription != null) {
                    var subBillingReference = new BillingReferenceModel() {
                        Id = Guid.NewGuid(),
                        BillingId = entity.Subscription.BillingReference.BillingId,
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
                            next_payment, 
                            trial_start, 
                            trial_end, 
                            cancelling_at_period_end
                        ) 
                        values (
                            @Id, 
                            @PlanId, 
                            @PriceBillingId, 
                            @CustomerId, 
                            @BillingReferenceId, 
                            @Status, 
                            @NextPayment, 
                            @TrialStart, 
                            @TrialEnd, 
                            @CancellingAtPeriodEnd
                        );",
                        new SubscriptionModel() {
                            Id = entity.Subscription.Id,
                            PlanId = entity.Subscription.PlanReference.PlanId,
                            PriceBillingId = entity.Subscription.PlanReference.PriceBillingId,
                            CustomerId = entity.Id,
                            BillingReferenceId = subBillingReference.Id,
                            Status = entity.Subscription.Status,
                            NextPayment = entity.Subscription.NextPayment,
                            TrialStart = entity.Subscription.TrialStart,
                            TrialEnd = entity.Subscription.TrialEnd,
                            CancellingAtPeriodEnd = entity.Subscription.CancellingAtPeriodEnd
                        }
                    );
                }

                t.Commit();
            }
        }

        public async Task Update(Customer customer) {
            using (var t = Connection.BeginTransaction()) {
                // Delete any old payment methods
                var oldPaymentMethods = await Connection.QueryAsync<PaymentMethodModel>("select * from payment_methods where customer_id = @Id;", customer);
                await Connection.ExecuteAsync("delete from payment_methods where customer_id = @Id", customer);
                await Connection.ExecuteAsync("delete from billing_references where id = @BillingReferenceId;", oldPaymentMethods.ToList());

                if (customer.PaymentMethods.Count > 0) {
                    var paymentMethodBillingReferences = new List<BillingReferenceModel>();
                    var paymentMethods = new List<PaymentMethodModel>();

                    foreach (PaymentMethod pm in customer.PaymentMethods) {
                        var brModel = new BillingReferenceModel() {
                            Id = Guid.NewGuid(),
                            BillingId = pm.BillingReference.BillingId,
                            Type = BillingReferenceType.PaymentMethod
                        };

                        var pModel = new PaymentMethodModel() {
                            Id = pm.Id,
                            BillingReferenceId = brModel.Id,
                            CustomerId = customer.Id,
                            Brand = pm.Brand,
                            Last4 = pm.Last4,
                            IsDefault = pm.IsDefault
                        };

                        paymentMethodBillingReferences.Add(brModel);
                        paymentMethods.Add(pModel);
                    }

                    await Connection.ExecuteAsync(
                        @"insert into billing_references (id, billing_id, type) values (@Id, @BillingId, @Type);",
                        paymentMethodBillingReferences
                    );

                    await Connection.ExecuteAsync(
                        @"insert into payment_methods (
                            id, 
                            customer_id, 
                            brand, 
                            last_4, 
                            billing_reference_id
                        ) values (
                            @Id, 
                            @CustomerId, 
                            @Brand, 
                            @Last4, 
                            @BillingReferenceId
                        );",
                        paymentMethods
                    );
                }

                // Delete subscription
                var oldSubscription = await Connection.QueryFirstOrDefaultAsync<SubscriptionModel>("select * from subscriptions where customer_id = @Id;", customer);
                await Connection.ExecuteAsync(@"delete from subscriptions where id = @Id;", oldSubscription);
                await Connection.ExecuteAsync(@"delete from billing_references where id = @BillingReferenceId;", oldSubscription);

                // Insert subscription (if applicable)
                if (customer.Subscription != null) {
                    var subBillingReference = new BillingReferenceModel() {
                        Id = Guid.NewGuid(),
                        BillingId = customer.Subscription.BillingReference.BillingId,
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
                            next_payment, 
                            trial_start, 
                            trial_end, 
                            cancelling_at_period_end
                        ) 
                        values (
                            @Id, 
                            @PlanId, 
                            @PriceBillingId, 
                            @CustomerId, 
                            @BillingReferenceId, 
                            @Status, 
                            @NextPayment, 
                            @TrialStart, 
                            @TrialEnd, 
                            @CancellingAtPeriodEnd
                        );",
                        new SubscriptionModel() {
                            Id = customer.Subscription.Id,
                            PlanId = customer.Subscription.PlanReference.PlanId,
                            PriceBillingId = customer.Subscription.PlanReference.PriceBillingId,
                            CustomerId = customer.Id,
                            BillingReferenceId = subBillingReference.Id,
                            Status = customer.Subscription.Status,
                            NextPayment = customer.Subscription.NextPayment,
                            TrialStart = customer.Subscription.TrialStart,
                            TrialEnd = customer.Subscription.TrialEnd,
                            CancellingAtPeriodEnd = customer.Subscription.CancellingAtPeriodEnd
                        }
                    );
                }

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
                    s.NextPayment,
                    s.TrialStart,
                    s.TrialEnd,
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
                    BillingReference.PaymentMethod(br.BillingId)
                )
            ).ToList();

            return customer;
        }
    }
}
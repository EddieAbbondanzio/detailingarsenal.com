using System;
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
                @"select s.*, br.* from subscriptions s
                  left join billing_references br on s.billing_reference_id = br.id
                  left join customers c on s.customer_id = c.id
                  where c.id = @Id;
                  
                  select c.*, br.* from customers c
                  left join billing_references br on c.billing_reference_id = br.id
                  where c.id = @Id;
                  
                  select pm.* from payment_methods pm
                  inner join customers c on pm.customer_id = c.id
                  where c.id = @Id;
                  ",
                  new {
                      Id = id
                  }
            )) {
                var subscription = reader.Read<SubscriptionModel, BillingReferenceModel, Subscription>(
                    (s, br) => new Subscription {
                        Id = s.Id,
                        PlanReference = new SubscriptionPlanReference(s.PlanId, s.PriceBillingId),
                        Status = s.Status,
                        TrialStart = s.TrialStart,
                        TrialEnd = s.TrialEnd,
                        BillingReference = new BillingReference(
                            br.BillingId,
                            br.Type
                        )
                    }
                ).FirstOrDefault();

                if (subscription == null) {
                    return null;
                }

                var customer = reader.Read<CustomerModel, BillingReferenceModel, Customer>(
                    (c, br) => new Customer {
                        Id = c.Id,
                        UserId = c.UserId,
                        BillingReference = new BillingReference(
                            br.BillingId,
                            br.Type
                        ),
                        Subscription = subscription
                    }
                ).First();

                customer.PaymentMethod = reader.Read<PaymentMethodModel>().Select(p => new PaymentMethod(p.Brand, p.Last4)).FirstOrDefault();

                return customer;
            }
        }

        public async Task<Customer?> FindByBillingId(string billingId) {
            using (var reader = await Connection.QueryMultipleAsync(
                @"select s.*, br.* from subscriptions s
                  left join billing_references sbr on s.billing_reference_id = sbr.id
                  left join customers c on s.customer_id = c.id
                  left join billing_references cbr on c.billing_reference_id = cbr.id
                  where cbr.billing_id = @BillingId;
                  
                  select c.*, br.* from customers c
                  left join billing_references br on c.billing_reference_id = br.id
                  where c.billing_id = @BillingId;
                  
                  select pm.* from payment_methods pm
                  inner join customers c on pm.customer_id = c.id
                  left join billing_references br on c.billing_reference_id = br.id
                  where c.billing_id = @BillingId;
                  ",
                  new {
                      BillingId = billingId
                  }
            )) {
                var subscription = reader.Read<SubscriptionModel, BillingReferenceModel, Subscription>(
                    (s, br) => new Subscription {
                        Id = s.Id,
                        PlanReference = new SubscriptionPlanReference(s.PlanId, s.PriceBillingId),
                        Status = s.Status,
                        TrialStart = s.TrialStart,
                        TrialEnd = s.TrialEnd,
                        BillingReference = new BillingReference(
                            br.BillingId,
                            br.Type
                        )
                    }
                ).FirstOrDefault();

                if (subscription == null) {
                    return null;
                }

                var customer = reader.Read<CustomerModel, BillingReferenceModel, Customer>(
                    (c, br) => new Customer {
                        Id = c.Id,
                        UserId = c.UserId,
                        BillingReference = new BillingReference(
                            br.BillingId,
                            br.Type
                        ),
                        Subscription = subscription
                    }
                ).First();

                customer.PaymentMethod = reader.Read<PaymentMethodModel>().Select(p => new PaymentMethod(p.Brand, p.Last4)).FirstOrDefault();

                return customer;
            }
        }

        public async Task<Customer?> FindByUser(User user) {
            using (var reader = await Connection.QueryMultipleAsync(
                @"select s.*, br.* from subscriptions s
                  left join billing_references br on s.billing_reference_id = br.id
                  left join customers c on s.customer_id = c.id
                  where c.user_id = @Id;
                  
                  select c.*, br.* from customers c
                  left join billing_references br on c.billing_reference_id = br.id
                  where c.user_id = @Id;
                  
                  select pm.* from payment_methods pm
                  inner join customers c on pm.customer_id = c.id
                  where c.user_id = @Id; 
                  ",
                user
            )) {
                var subscription = reader.Read<SubscriptionModel, BillingReferenceModel, Subscription>(
                    (s, br) => new Subscription {
                        Id = s.Id,
                        PlanReference = new SubscriptionPlanReference(s.PlanId, s.PriceBillingId),
                        Status = s.Status,
                        TrialStart = s.TrialStart,
                        TrialEnd = s.TrialEnd,
                        BillingReference = new BillingReference(
                            br.BillingId,
                            br.Type
                        )
                    }
                ).FirstOrDefault();

                if (subscription == null) {
                    return null;
                }

                var customer = reader.Read<CustomerModel, BillingReferenceModel, Customer>(
                    (c, br) => new Customer {
                        Id = c.Id,
                        UserId = c.UserId,
                        BillingReference = new BillingReference(
                            br.BillingId,
                            br.Type
                        ),
                        Subscription = subscription
                    }
                ).First();

                customer.PaymentMethod = reader.Read<PaymentMethodModel>().Select(p => new PaymentMethod(p.Brand, p.Last4)).FirstOrDefault();

                return customer;
            }
        }

        public async Task Add(Customer entity) {
            using (var t = Connection.BeginTransaction()) {
                var customerBillingReference = new BillingReferenceModel() {
                    Id = Guid.NewGuid(),
                    BillingId = entity.BillingReference.BillingId,
                    Type = entity.Subscription.BillingReference.Type
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

                if (entity.PaymentMethod != null) {
                    await Connection.ExecuteAsync(
                        @"insert into payment_methods (id, customer_id, brand, last_4) values (@Id, @CustomerId, @Brand, @Last4);",
                        new PaymentMethodModel() {
                            Id = Guid.NewGuid(),
                            CustomerId = entity.Id,
                            Brand = entity.PaymentMethod.Brand,
                            Last4 = entity.PaymentMethod.Last4
                        }
                    );
                }

                var subBillingReference = new BillingReferenceModel() {
                    Id = Guid.NewGuid(),
                    BillingId = entity.Subscription.BillingReference.BillingId,
                    Type = entity.Subscription.BillingReference.Type
                };


                await Connection.ExecuteAsync(
                    @"insert into billing_references (id, billing_id, type) values (@Id, @BillingId, @Type);",
                    subBillingReference
                );

                await Connection.ExecuteAsync(
                    @"insert into subscriptions 
                    (id, plan_id, price_billing_id, customer_id, billing_reference_id, status, next_payment, trial_start, trial_end) 
                    values (@Id, @PlanId, @PriceBillingId, @CustomerId, @BillingReferenceId, @Status, @NextPayment, @TrialStart, @TrialEnd);",
                    new SubscriptionModel() {
                        Id = entity.Subscription.Id,
                        PlanId = entity.Subscription.PlanReference.PlanId,
                        PriceBillingId = entity.Subscription.PlanReference.PriceBillingId,
                        CustomerId = entity.Id,
                        BillingReferenceId = subBillingReference.Id,
                        Status = entity.Subscription.Status,
                        NextPayment = entity.Subscription.NextPayment,
                        TrialStart = entity.Subscription.TrialStart,
                        TrialEnd = entity.Subscription.TrialEnd
                    }
                );

                t.Commit();
            }
        }

        public async Task Update(Customer entity) {
            using (var t = Connection.BeginTransaction()) {
                // Query off the DB since we can't trust anything but the ID of the customer to change.
                using (var reader = await Connection.QueryMultipleAsync(
                    @"select * from customers where id = @Id;
                      select s.* from subscriptions s left join customers c on s.customer_id = c.id where c.id = @Id;  
                    ",
                    entity
                )) {

                    var customerModel = reader.Read<CustomerModel>().First();
                    var subscriptionModel = reader.Read<SubscriptionModel>().First();

                    // Update the subscription
                    await Connection.ExecuteAsync(
                        @"update subscriptions set 
                        plan_id = @PlanId, 
                        price_billing_id = @PriceBillingId, 
                        status = @Status, 
                        next_payment = @NextPayment,
                        trial_start = @TrialStart, 
                        trial_end = @TrialEnd,
                        customer_id = @CustomerId 
                        where id = @Id",
                        new SubscriptionModel {
                            PlanId = entity.Subscription.PlanReference.PlanId,
                            Status = entity.Subscription.Status,
                            NextPayment = entity.Subscription.NextPayment,
                            TrialStart = entity.Subscription.TrialStart,
                            TrialEnd = entity.Subscription.TrialEnd,
                            PriceBillingId = entity.Subscription.PlanReference.PriceBillingId,
                            CustomerId = entity.Id
                        }
                    );

                    // Update the subscription billing reference
                    await Connection.ExecuteAsync(
                        @"update billing_references set billing_id = @BillingId where id = @Id",
                        new {
                            Id = subscriptionModel.BillingReferenceId,
                            BillingId = entity.Subscription.BillingReference.BillingId
                        }
                    );

                    await Connection.ExecuteAsync(
                        @"delete from payment_methods where customer_id = @Id",
                        entity
                    );

                    if (entity.PaymentMethod != null) {
                        await Connection.ExecuteAsync(
                            @"insert into payment_methods (id, customer_id, brand, last_4) values (@Id, @CustomerId, @Brand, @Last4);",
                            new PaymentMethodModel() {
                                Id = Guid.NewGuid(),
                                CustomerId = entity.Id,
                                Brand = entity.PaymentMethod.Brand,
                                Last4 = entity.PaymentMethod.Last4
                            }
                        );
                    }

                    // Update the customer
                    await Connection.ExecuteAsync(
                        @"update customers set user_id = @UserId where id = @Id",
                        new {
                            Id = customerModel.Id,
                            UserId = entity.UserId,
                        }
                    );

                    // Update the customer billing reference
                    await Connection.ExecuteAsync(
                        @"update billing_references set billing_id = @BillingId where id = @Id",
                        new {
                            Id = customerModel.BillingReferenceId,
                            BillingId = entity.BillingReference.BillingId
                        }
                    );

                    t.Commit();
                }
            }
        }

        public async Task Delete(Customer entity) {
            /*
                This will probably throw an error if ever used and the entity was updated...
                Gotta query out some data to work with first. (IE Customer)
            */

            using (var t = Connection.BeginTransaction()) {
                // delete subscription
                await Connection.ExecuteAsync(
                    @"delete from subscriptions where id = @Id",
                    entity.Subscription
                );

                // delete subscription billing reference
                await Connection.ExecuteAsync(
                    @"delete from billing_references where billing_id = @BillingId",
                    entity.Subscription.BillingReference
                );

                await Connection.ExecuteAsync(
                    @"delete from payment_methods where customer_id = @Id",
                    entity
                );

                // delete customer
                await Connection.ExecuteAsync(
                    @"delete from customers where id = @Id",
                    entity
                );

                // delete customer billing reference
                await Connection.ExecuteAsync(
                    @"delete from billing_references where billing_id = @BillingId",
                    entity.BillingReference
                );

                t.Commit();
            }
        }
    }
}
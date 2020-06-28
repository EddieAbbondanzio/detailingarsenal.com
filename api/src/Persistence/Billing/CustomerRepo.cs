using System;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;

namespace DetailingArsenal.Persistence.Billing {
    public class CustomerRepo : DatabaseInteractor, ICustomerRepo {
        public CustomerRepo(IDatabase database) : base(database) { }

        public async Task<Customer?> FindById(Guid id) {
            var c = await Connection.QueryFirstOrDefaultAsync<Customer>(
                @"select * from customers where id = @Id;", new { Id = id }
            );

            if (c == null) {
                return null;
            }

            c.Subscription = await Connection.QueryFirstAsync<Subscription>(
                @"select * from subscriptions where customer_id = @Id;",
                c
            );

            return c;
        }

        public async Task Add(Customer entity) {
            using (var t = Connection.BeginTransaction()) {
                await Connection.ExecuteAsync(
                    @"insert into customers (id, user_id, external_id) values (@Id, @UserId, @ExternalId);",
                    entity
                );

                await Connection.ExecuteAsync(
                    @"insert into subscriptions (id, customer_id, plan_id, external_id, status) values (@Id, @CustomerId, @PlanId, @ExternalId, @Status);",
                    entity.Subscription
                );

                await t.CommitAsync();
            }
        }

        public async Task Update(Customer entity) {
            using (var t = Connection.BeginTransaction()) {
                await Connection.ExecuteAsync(
                    @"update customers set user_id = @UserId, external_id = @ExternalId where id = @Id;",
                    entity
                );

                await Connection.ExecuteAsync(
                    @"update subscriptions set customer_id = @CustomerId, plan_id = @PlanId, external_id = @ExternalId, status = @Status where id = @Id;",
                    entity.Subscription
                );

                await t.CommitAsync();
            }
        }

        public async Task Delete(Customer entity) {
            using (var t = Connection.BeginTransaction()) {
                await Connection.ExecuteAsync(
                    @"delete from subscriptions where customer_id = @Id;",
                    entity
                );

                await Connection.ExecuteAsync(
                    @"delete from customer where id = @Id",
                    entity
                );
                await t.CommitAsync();
            }
        }
    }
}
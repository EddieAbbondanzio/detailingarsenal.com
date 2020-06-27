using System;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Persistence.Billing {
    public class SubscriptionRepo : DatabaseInteractor, ISubscriptionRepo {
        public SubscriptionRepo(IDatabase database) : base(database) { }

        public async Task<Subscription?> FindById(Guid id) {
            return await Connection.QueryFirstOrDefaultAsync<Subscription>(
                @"select * from subscriptions where id = @Id",
                new { Id = id }
            );
        }

        public async Task Add(Subscription entity) {
            await Connection.ExecuteAsync(
                @"insert into subscriptions (id, user_id, plan_id, external_id, status) values (@Id, @UserId, @PlanId, @ExternalId, @Status);",
                entity
            );
        }

        public async Task Update(Subscription entity) {
            await Connection.ExecuteAsync(
                @"update subscriptions set plan_id = @PlanId, external_id = @ExternalId, status = @Status where id = @Id;",
                entity
            );
        }

        public async Task Delete(Subscription entity) {
            await Connection.ExecuteAsync(
                @"delete from subscriptions where id = @Id", entity
            );
        }
    }
}
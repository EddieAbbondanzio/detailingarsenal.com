using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Infrastructure.Persistence {
    public class HoursOfOperationRepo : DatabaseInteractor, IHoursOfOperationRepo {
        public HoursOfOperationRepo(IDatabase database) : base(database) {
        }

        public async Task<HoursOfOperation?> FindById(Guid id) {
            var hours = await Connection.QueryFirstOrDefaultAsync<HoursOfOperation>(
                @"select * from hours_of_operations where id = @Id",
                new { Id = id }
            );

            if (hours == null) {
                return null;
            }

            hours.Days = (await Connection.QueryAsync<HoursOfOperationDay>(
                @"select * from hours_of_operation_days where hours_of_operation_id = @Id",
                hours
            )).ToList();

            return hours;
        }

        public async Task<HoursOfOperation> FindForUser(User user) {
            var hours = await Connection.QueryFirstAsync<HoursOfOperation>(
                @"select * from hours_of_operations where user_id = @Id",
                user
            );

            hours.Days = (await Connection.QueryAsync<HoursOfOperationDay>(
                @"select * from hours_of_operation_days where hours_of_operation_id = @Id",
                hours
            )).ToList();

            return hours;
        }

        public async Task Add(HoursOfOperation entity) {
            await Connection.ExecuteAsync(
                @"insert into hours_of_operations (id, user_id) values (@Id, @UserId);",
                entity
            );

            await Connection.ExecuteAsync(
                @"insert into hours_of_operation_days (id, hours_of_operation_id, day, open, close, enabled) values (@Id, @HoursOfOperationId, @Day, @Open, @Close, @Enabled);",
                entity.Days
            );
        }

        public async Task Update(HoursOfOperation entity) {
            await Connection.ExecuteAsync(@"update hours_of_operations set user_id = @UserId where id = @Id", entity);

            await Connection.ExecuteAsync("delete from hours_of_operation_days where hours_of_operation_id = @Id", entity);
            await Connection.ExecuteAsync(
                @"insert into hours_of_operation_days (id, hours_of_operation_id, day, open, close, enabled) values (@Id, @HoursOfOperationId, @Day, @Open, @Close, @Enabled);",
                entity.Days
            );
        }

        public async Task Delete(HoursOfOperation entity) {
            await Connection.ExecuteAsync("delete from hours_of_operation_days where hours_of_operation_id = @Id", entity);
            await Connection.ExecuteAsync("delete from hours_of_operations where id = @Id", entity);
        }
    }
}
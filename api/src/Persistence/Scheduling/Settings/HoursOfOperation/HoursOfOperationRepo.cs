using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Persistence.Settings {
    [DependencyInjection(RegisterAs = typeof(IHoursOfOperationRepo))]
    public class HoursOfOperationRepo : DatabaseInteractor, IHoursOfOperationRepo {
        public HoursOfOperationRepo(IDatabase database) : base(database) {
        }

        public async Task<HoursOfOperation?> FindById(Guid id) {
            using (var conn = OpenConnection()) {
                var hours = await conn.QueryFirstOrDefaultAsync<HoursOfOperation>(
                    @"select * from hours_of_operations where id = @Id",
                    new { Id = id }
                );

                if (hours == null) {
                    return null;
                }

                hours.Days = (await conn.QueryAsync<HoursOfOperationDay>(
                    @"select * from hours_of_operation_days where hours_of_operation_id = @Id",
                    hours
                )).ToList();

                return hours;
            }
        }

        public async Task<HoursOfOperation?> FindForUser(User user) {
            using (var conn = OpenConnection()) {
                var hours = await conn.QueryFirstOrDefaultAsync<HoursOfOperation>(
                    @"select * from hours_of_operations where user_id = @Id",
                    user
                );

                if (hours == null) {
                    return null;
                }

                hours.Days = (await conn.QueryAsync<HoursOfOperationDay>(
                    @"select * from hours_of_operation_days where hours_of_operation_id = @Id",
                    hours
                )).ToList();

                return hours;
            }
        }

        public async Task Add(HoursOfOperation entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"insert into hours_of_operations (id, user_id) values (@Id, @UserId);",
                        entity
                    );

                    var days = entity.Days.Select(d => new HoursOfOperationDayModel {
                        Id = d.Id,
                        HoursOfOperationId = entity.Id,
                        Day = d.Day,
                        Open = d.Day,
                        Close = d.Close,
                        Enabled = d.Enabled
                    });

                    await conn.ExecuteAsync(
                        @"insert into hours_of_operation_days (id, hours_of_operation_id, day, open, close, enabled) values (@Id, @HoursOfOperationId, @Day, @Open, @Close, @Enabled);",
                        days
                    );

                    t.Commit();
                }
            }
        }

        public async Task Update(HoursOfOperation entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"update hours_of_operations set user_id = @UserId where id = @Id",
                        entity
                    );

                    await conn.ExecuteAsync(
                        @"delete from hours_of_operation_days where hours_of_operation_id = @Id",
                        entity
                    );

                    var days = entity.Days.Select(d => new HoursOfOperationDayModel {
                        Id = d.Id,
                        HoursOfOperationId = entity.Id,
                        Day = d.Day,
                        Open = d.Day,
                        Close = d.Close,
                        Enabled = d.Enabled
                    });

                    await conn.ExecuteAsync(
                        @"insert into hours_of_operation_days (id, hours_of_operation_id, day, open, close, enabled) values (@Id, @HoursOfOperationId, @Day, @Open, @Close, @Enabled);",
                        days
                    );

                    t.Commit();
                }
            }
        }

        public async Task Delete(HoursOfOperation entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"delete from hours_of_operation_days where hours_of_operation_id = @Id",
                        entity
                    );

                    await conn.ExecuteAsync(
                        @"delete from hours_of_operations where id = @Id",
                        entity
                    );

                    t.Commit();
                }
            }
        }
    }
}
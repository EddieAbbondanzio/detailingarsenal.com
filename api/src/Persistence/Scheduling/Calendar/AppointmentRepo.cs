using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Calendar;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Persistence.Calendar {
    public class AppointmentRepo : DatabaseInteractor, IAppointmentRepo {
        public AppointmentRepo(IDatabase database) : base(database) { }

        public async Task<Appointment?> FindById(Guid id) {
            using (var conn = OpenConnection()) {
                var appointment = await conn.QueryFirstOrDefaultAsync<Appointment>(
                    @"select * from appointments where id = @Id", new { Id = id }
                );

                if (appointment == null) {
                    return null;
                }

                appointment.Blocks = (await conn.QueryAsync<AppointmentBlock>(
                    @"select id, start_date as start, end_date as end, appointment_id from appointment_blocks where appointment_id = @Id",
                    appointment
                )).ToList();

                return appointment;
            }
        }

        public async Task<List<Appointment>> FindForDay(DateTime date, User user) {
            using (var conn = OpenConnection()) {
                var ids = await conn.QueryAsync<Guid>(
                    @"select distinct on (appointment_id) appointment_id from appointment_blocks where cast(start_date as date) = @Date",
                    new { Date = date }
                );

                var appointments = (await conn.QueryAsync<Appointment>(
                    @"select * from appointments where id = any (@Ids)",
                    new { Ids = ids }
                )).ToList();

                foreach (Appointment appointment in appointments) {
                    appointment.Blocks = (await conn.QueryAsync<AppointmentBlock>(
                        @"select id, start_date as start, end_date as end, appointment_id from appointment_blocks where appointment_id = @Id",
                        appointment
                    )).ToList();
                }

                return appointments;
            }
        }

        public async Task<List<Appointment>> FindForWeek(DateTime date, User user) {
            using (var conn = OpenConnection()) {
                var sunday = date.AddDays(-(int)date.DayOfWeek);
                var saturday = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Saturday);

                var ids = await conn.QueryAsync<Guid>(
                    @"select distinct on (appointment_id) appointment_id from appointment_blocks where cast(start_date as date) >= @Sunday and cast(start_date as date) <= @Saturday",
                    new { Sunday = sunday, Saturday = saturday }
                );

                var appointments = (await conn.QueryAsync<Appointment>(
                    @"select * from appointments where id = any (@Ids)",
                    new { Ids = ids }
                )).ToList();

                foreach (Appointment appointment in appointments) {
                    appointment.Blocks = (await conn.QueryAsync<AppointmentBlock>(
                        @"select id, start_date as start, end_date as end, appointment_id from appointment_blocks where appointment_id = @Id",
                        appointment
                    )).ToList();
                }

                return appointments;
            }
        }

        public async Task Add(Appointment entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"insert into appointments 
                (id, user_id, service_id, client_id, price, notes)
                values (@Id, @UserId, @ServiceId, @ClientId, @Price, @Notes);",
                        entity
                    );

                    await conn.ExecuteAsync(
                        @"insert into appointment_blocks
                (id, appointment_id, start_date, end_date)
                values (@Id, @AppointmentId, @Start, @End)",
                        entity.Blocks
                    );
                    t.Commit();
                }
            }
        }

        public async Task Update(Appointment entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync(
                        @"update appointments set
                user_id = @UserId, service_id = @ServiceId, client_id = @ClientId, price = @Price, notes = @Notes
                 where id = @Id",
                        entity
                    );

                    // It's not worth trying to save appointment block ids.
                    await conn.ExecuteAsync("delete from appointment_blocks where appointment_id = @Id", entity);
                    await conn.ExecuteAsync(
                        @"insert into appointment_blocks
                (id, appointment_id, start_date, end_date)
                values (@Id, @AppointmentId, @Start, @End)",
                        entity.Blocks
                    );
                    t.Commit();
                }
            }
        }

        public async Task Delete(Appointment entity) {
            using (var conn = OpenConnection()) {
                using (var t = conn.BeginTransaction()) {
                    await conn.ExecuteAsync("delete from appointment_blocks where appointment_id = @Id", entity);
                    await conn.ExecuteAsync("delete from appointments where id = @Id", entity);
                    t.Commit();
                }
            }
        }

        public async Task<int> CountForService(Service service) {
            using (var conn = OpenConnection()) {
                return await conn.ExecuteScalarAsync<int>(@"select count(*) from appointments where service_id = @Id", service);
            }
        }


        public async Task<int> CountForClient(Client client) {
            using (var conn = OpenConnection()) {
                return await conn.ExecuteScalarAsync<int>(@"select count(*) from appointments where client_id = @Id", client);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Infrastructure.Persistence {
    public class AppointmentRepo : DatabaseInteractor, IAppointmentRepo {
        public AppointmentRepo(IDatabase database) : base(database) { }

        public async Task<Appointment?> FindById(Guid id) {
            var appointment = await Connection.QueryFirstOrDefaultAsync<Appointment>(
                @"select * from appointments where id = @Id", new { Id = id }
            );

            if (appointment == null) {
                return null;
            }

            appointment.Blocks = (await Connection.QueryAsync<AppointmentBlock>(
                @"select * from appointment_blocks where appointment_id = @Id",
                appointment
            )).ToList();

            return appointment;
        }

        public async Task<List<Appointment>> FindForDay(DateTime date, User user) {
            var ids = await Connection.QueryAsync<Guid>(
                @"select distinct on (appointment_id) appointment_id from appointment_blocks where date = @Date",
                new { Date = date }
            );

            var appointments = (await Connection.QueryAsync<Appointment>(
                @"select * from appointments where id = any (@Ids)",
                new { Ids = ids }
            )).ToList();

            foreach (Appointment appointment in appointments) {
                appointment.Blocks = (await Connection.QueryAsync<AppointmentBlock>(
                    @"select * from appointment_blocks where appointment_id = @Id",
                    appointment
                )).ToList();
            }

            return appointments;
        }

        public async Task<List<Appointment>> FindForWeek(DateTime date, User user) {
            var sunday = date.AddDays(-(int)date.DayOfWeek);
            var saturday = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Saturday);

            var ids = await Connection.QueryAsync<Guid>(
                @"select distinct on (appointment_id) appointment_id from appointment_blocks where date >= @Sunday and date <= @Saturday",
                new { Sunday = sunday, Saturday = saturday }
            );

            var appointments = (await Connection.QueryAsync<Appointment>(
                @"select * from appointments where id = any (@Ids)",
                new { Ids = ids }
            )).ToList();

            foreach (Appointment appointment in appointments) {
                appointment.Blocks = (await Connection.QueryAsync<AppointmentBlock>(
                    @"select * from appointment_blocks where appointment_id = @Id",
                    appointment
                )).ToList();
            }

            return appointments;
        }

        public async Task Add(Appointment entity) {
            await Connection.ExecuteAsync(
                @"insert into appointments 
                (id, user_id, service_id, client_id, price, notes)
                values (@Id, @UserId, @ServiceId, @ClientId, @Price, @Notes);",
                entity
            );

            await Connection.ExecuteAsync(
                @"insert into appointment_blocks
                (id, appointment_id, date, time, duration)
                values (@Id, @AppointmentId, @Date, @Time, @Duration)",
                entity.Blocks
            );
        }

        public async Task Update(Appointment entity) {
            await Connection.ExecuteAsync(
                @"update appointments set
                user_id = @UserId, service_id = @ServiceId, client_id = @ClientId, price = @Price, notes = @Notes
                 where id = @Id",
                entity
            );

            // It's not worth trying to save appointment block ids.
            await Connection.ExecuteAsync("delete from appointment_blocks where appointment_id = @Id", entity);
            await Connection.ExecuteAsync(
                @"insert into appointment_blocks
                (id, appointment_id, date, time, duration)
                values (@Id, @AppointmentId, @Date, @Time, @Duration)",
                entity.Blocks
            );
        }

        public async Task Delete(Appointment entity) {
            await Connection.ExecuteAsync("delete from appointment_blocks where appointment_id = @Id", entity);
            await Connection.ExecuteAsync("delete from appointments where id = @Id", entity);
        }

        public async Task<int> CountForService(Service service) {
            return await Connection.ExecuteScalarAsync<int>(@"select count(*) from appointments where service_id = @Id", service);
        }


        public async Task<int> CountForClient(Client client) {
            return await Connection.ExecuteScalarAsync<int>(@"select count(*) from appointments where client_id = @Id", client);
        }
    }
}
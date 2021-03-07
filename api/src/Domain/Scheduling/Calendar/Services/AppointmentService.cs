using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Calendar {
    public interface IAppointmentService : IService {
        Task<Appointment> GetById(Guid id);
        Task<List<Appointment>> GetWithinRange(DateTime date, AppointmentRange range, User user);
        Task<Appointment> Create(AppointmentCreate create, User user);
        Task Update(Appointment appointment, AppointmentUpdate update);
        Task Delete(Appointment appointment);
    }

    [DependencyInjection(RegisterAs = typeof(IAppointmentService))]
    public class AppointmentService : IAppointmentService {
        IAppointmentRepo repo;

        public AppointmentService(IAppointmentRepo repo) {
            this.repo = repo;
        }

        public async Task<Appointment> GetById(Guid id) {
            return await repo.FindById(id) ?? throw new EntityNotFoundException();
        }

        public async Task<List<Appointment>> GetWithinRange(DateTime date, AppointmentRange range, User user) {
            if (range == AppointmentRange.Day) {
                return await repo.FindForDay(date, user);
            } else {
                return await repo.FindForWeek(date, user);
            }
        }

        public async Task<Appointment> Create(AppointmentCreate create, User user) {
            var appointment = Appointment.Create(
                user.Id,
                create.ServiceId,
                create.ClientId,
                create.Price,
                create.Notes
            );

            appointment.Blocks = create.Blocks.Select(b => AppointmentBlock.Create(
                appointment.Id,
                b.Start,
                b.End
            )).ToList();

            await repo.Add(appointment);
            return appointment;
        }

        public async Task Update(Appointment appointment, AppointmentUpdate update) {
            appointment.ServiceId = update.ServiceId;
            appointment.ClientId = update.ClientId;
            appointment.Price = update.Price;
            appointment.Notes = update.Notes;

            appointment.Blocks = update.Blocks.Select(b => AppointmentBlock.Create(
                appointment.Id,
                b.Start,
                b.End
            )).ToList();

            await repo.Update(appointment);
        }

        public async Task Delete(Appointment appointment) {
            await repo.Delete(appointment);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Clients;
using DetailingArsenal.Domain.Settings;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Calendar {
    public interface IAppointmentRepo : IRepo<Appointment> {
        Task<List<Appointment>> FindForDay(DateTime date, User user);
        Task<List<Appointment>> FindForWeek(DateTime date, User user);

        Task<int> CountForService(Service service);
        Task<int> CountForClient(Client client);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface IAppointmentRepo : IRepo<Appointment> {
        Task<List<Appointment>> FindForDay(DateTime date, User user);
        Task<List<Appointment>> FindForWeek(DateTime date, User user);

        Task<int> CountForService(Service service);
        Task<int> CountForClient(Client client);
    }
}
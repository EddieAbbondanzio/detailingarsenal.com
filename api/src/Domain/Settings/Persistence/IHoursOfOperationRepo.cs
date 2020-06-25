using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Settings {
    public interface IHoursOfOperationRepo : IRepo<HoursOfOperation> {
        Task<HoursOfOperation> FindForUser(User user);
    }
}
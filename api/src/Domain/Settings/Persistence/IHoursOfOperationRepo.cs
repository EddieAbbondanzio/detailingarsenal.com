using System;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain.Settings {
    public interface IHoursOfOperationRepo : IRepo<HoursOfOperation> {
        Task<HoursOfOperation> FindForUser(User user);
    }
}
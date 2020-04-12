using System;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface IHoursOfOperationRepo : IRepo<HoursOfOperation> {
        Task<HoursOfOperation> FindForUser(User user);
    }
}
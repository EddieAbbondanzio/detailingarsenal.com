using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Settings {
    public interface IServiceRepo : IRepo<Service> {
        Task<List<Service>> FindByUser(User user);
        Task<Service?> FindByName(string name);
    }
}
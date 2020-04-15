using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface IClientRepo : IRepo<Client> {
        Task<List<Client>> FindByUser(User user);
    }
}
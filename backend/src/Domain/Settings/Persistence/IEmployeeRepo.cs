using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface IEmployeeRepo : IRepo<Employee> {
        Task<List<Employee>> FindByUser(User user);
    }
}
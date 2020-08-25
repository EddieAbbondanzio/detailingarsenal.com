using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Billing {
    public interface ICustomerReader : IReader<CustomerReadModel> {
        Task<CustomerReadModel> ReadForUser(User user);
    }
}
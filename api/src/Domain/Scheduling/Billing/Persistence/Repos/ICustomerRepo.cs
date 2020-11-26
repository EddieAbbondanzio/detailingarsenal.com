using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    public interface ICustomerRepo : IRepo<Customer> {
        Task<Customer?> FindByUser(User user);
        Task<Customer?> FindByBillingId(string billingId);
    }
}
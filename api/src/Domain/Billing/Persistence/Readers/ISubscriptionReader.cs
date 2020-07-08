using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Billing {
    public interface ISubscriptionReader : IReader {
        Task<SubscriptionReadModel> ReadForUser(User user);
    }
}
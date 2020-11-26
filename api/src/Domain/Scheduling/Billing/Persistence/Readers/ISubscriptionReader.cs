using System.Threading.Tasks;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Scheduling.Billing {
    public interface ISubscriptionReader : IReader<SubscriptionReadModel> {
        Task<SubscriptionReadModel> ReadForUser(User user);
    }
}
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    public interface ISubscriptionReader : IReader<SubscriptionReadModel> {
        Task<SubscriptionReadModel> ReadForUser(User user);
    }
}
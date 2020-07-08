using System.Threading.Tasks;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Billing {
    [Authorization]
    public class GetUserSubscriptionHandler : ActionHandler<GetUserSubscriptionQuery, SubscriptionReadModel> {
        ISubscriptionReader subscriptionReader;

        public GetUserSubscriptionHandler(ISubscriptionReader subscriptionReader) {
            this.subscriptionReader = subscriptionReader;
        }

        public async override Task<SubscriptionReadModel> Execute(GetUserSubscriptionQuery input, User? user) {
            var sub = await subscriptionReader.ReadForUser(user!);
            return sub;
        }
    }
}
using System;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Application {
    /// <summary>
    /// When a new user is generated, go ahead and create an empty business for the user.
    /// </summary>
    public class NewUserBusinessCreator : IBusEventHandler<NewUserEvent> {
        private IBusinessRepo repo;

        public NewUserBusinessCreator(IBusinessRepo repo) {
            this.repo = repo;
        }

        public async Task Handle(NewUserEvent busEvent) {
            var b = new Business() {
                Id = Guid.NewGuid(),
                UserId = busEvent.User.Id
            };

            await repo.Add(b);
        }
    }
}
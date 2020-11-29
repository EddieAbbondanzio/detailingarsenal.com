using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users {
    public class UserUpdateHandler : ActionHandler<UserUpdateCommand> {
        private IUserRepo repo;

        public UserUpdateHandler(IUserRepo repo) {
            this.repo = repo;
        }

        public async override Task Execute(UserUpdateCommand input, User? user) {
            user!.Name = input.Name;
            await repo.Update(user);
        }
    }
}
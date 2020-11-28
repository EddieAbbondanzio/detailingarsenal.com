using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users {
    public class UserUpdateHandler : ActionHandler<UserUpdateCommand, CommandResult> {
        private IUserRepo repo;

        public UserUpdateHandler(IUserRepo repo) {
            this.repo = repo;
        }

        public async override Task<CommandResult> Execute(UserUpdateCommand input, User? user) {
            user!.Name = input.Name;
            await repo.Update(user);

            return CommandResult.Success();
        }
    }
}
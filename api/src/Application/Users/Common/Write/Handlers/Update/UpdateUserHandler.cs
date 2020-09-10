using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users {
    public class UpdateUserHandler : ActionHandler<UpdateUserCommand, CommandResult> {
        private IUserRepo repo;

        public UpdateUserHandler(IUserRepo repo) {
            this.repo = repo;
        }

        public async override Task<CommandResult> Execute(UpdateUserCommand input, User? user) {
            user!.Name = input.Name;
            await repo.Update(user);

            return CommandResult.Success();
        }
    }
}
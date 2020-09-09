using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users {
    public class UpdateUserHandler : ActionHandler<UpdateUserCommand> {
        private IUserRepo repo;

        public UpdateUserHandler(IUserRepo repo) {
            this.repo = repo;
        }

        public async override Task Execute(UpdateUserCommand input, User? user) {
            user!.Name = input.Name;
            await repo.Update(user);
        }
    }
}
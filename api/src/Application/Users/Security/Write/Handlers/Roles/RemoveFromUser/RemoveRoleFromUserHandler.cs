using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Authorization(Action = "remove-role", Scope = "users")]
    [DependencyInjection]
    public class RemoveRoleFromUserHandler : ActionHandler<RemoveRoleFromUserCommand> {
        IUserRepo userRepo;
        IRoleAssigner roleAssigner;

        public RemoveRoleFromUserHandler(IUserRepo userRepo, IRoleAssigner roleAssigner) {
            this.userRepo = userRepo;
            this.roleAssigner = roleAssigner;
        }

        public async override Task Execute(RemoveRoleFromUserCommand input, User? user) {
            var userToRemoveFrom = await userRepo.FindById(input.UserId) ?? throw new EntityNotFoundException();
            await roleAssigner.RemoveRole(userToRemoveFrom, input.RoleId);
        }
    }
}
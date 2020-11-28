using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Authorization(Action = "add-role", Scope = "users")]
    public class AddRoleToUserHandler : ActionHandler<AddRoleToUserCommand> {
        IUserRepo userRepo;
        IRoleRepo roleRepo;

        public AddRoleToUserHandler(IUserRepo userRepo, IRoleRepo roleRepo) {
            this.userRepo = userRepo;
            this.roleRepo = roleRepo;
        }

        public async override Task Execute(AddRoleToUserCommand input, User? user) {
            var userToRemoveFrom = await userRepo.FindById(input.UserId) ?? throw new EntityNotFoundException();
            var role = await roleRepo.FindById(input.RoleId) ?? throw new EntityNotFoundException();

            await roleRepo.AddToUser(userToRemoveFrom, role);
        }
    }
}
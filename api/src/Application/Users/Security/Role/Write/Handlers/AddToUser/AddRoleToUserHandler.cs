using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Authorization(Action = "add-role", Scope = "users")]
    public class AddRoleToUserHandler : ActionHandler<AddRoleToUserCommand> {
        IUserRepo userRepo;
        IRoleService roleService;

        public AddRoleToUserHandler(IUserRepo userRepo, IRoleService roleService) {
            this.userRepo = userRepo;
            this.roleService = roleService;
        }

        public async override Task Execute(AddRoleToUserCommand input, User? user) {
            var userToRemoveFrom = await userRepo.FindById(input.UserId) ?? throw new EntityNotFoundException();
            var role = await roleService.GetById(input.RoleId);

            await roleService.RemoveRoleFromUser(role, userToRemoveFrom);
        }
    }
}
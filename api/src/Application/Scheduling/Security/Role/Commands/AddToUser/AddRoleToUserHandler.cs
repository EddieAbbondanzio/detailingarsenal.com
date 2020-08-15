using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Security {
    [Authorization(Action = "add-role", Scope = "users")]
    public class AddRoleToUserHandler : ActionHandler<AddRoleToUserCommand> {
        IUserService userService;
        IRoleService roleService;

        public AddRoleToUserHandler(IUserService userService, IRoleService roleService) {
            this.userService = userService;
            this.roleService = roleService;
        }

        public async override Task Execute(AddRoleToUserCommand input, User? user) {
            var userToRemoveFrom = await userService.GetUserById(input.UserId);
            var role = await roleService.GetById(input.RoleId);

            await roleService.RemoveRoleFromUser(role, userToRemoveFrom);
        }
    }
}
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Security {
    [Authorization(Action = "remove-role", Scope = "users")]
    public class RemoveRoleFromUserHandler : ActionHandler<RemoveRoleFromUserCommand> {
        IUserService userService;
        IRoleService roleService;

        public RemoveRoleFromUserHandler(IUserService userService, IRoleService roleService) {
            this.userService = userService;
            this.roleService = roleService;
        }

        public async override Task Execute(RemoveRoleFromUserCommand input, User? user) {
            var userToAddTo = await userService.GetUserById(input.UserId);
            var role = await roleService.GetById(input.RoleId);

            await roleService.AddRoleToUser(role, userToAddTo);
        }
    }
}
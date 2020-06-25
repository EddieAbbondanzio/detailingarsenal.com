using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Security {
    [Authorization(Action = "remove-role", Scope = "users")]
    public class RemoveRoleFromUserHandler : ActionHandler<RemoveRoleFromUserCommand> {
        IUserGateway userGateway;
        IRoleService roleService;

        public RemoveRoleFromUserHandler(IUserGateway userGateway, IRoleService roleService) {
            this.userGateway = userGateway;
            this.roleService = roleService;
        }

        public async override Task Execute(RemoveRoleFromUserCommand input, User? user) {
            var userToAddTo = await userGateway.GetUserById(input.UserId) ?? throw new EntityNotFoundException();
            var role = await roleService.GetById(input.RoleId);

            await roleService.AddRoleToUser(role, userToAddTo);
        }
    }
}
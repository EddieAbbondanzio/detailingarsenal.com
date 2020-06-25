using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Security {
    [Authorization(Action = "add-role", Scope = "users")]
    public class AddRoleToUserHandler : ActionHandler<AddRoleToUserCommand> {
        IUserGateway userGateway;
        IRoleService roleService;

        public AddRoleToUserHandler(IUserGateway userGateway, IRoleService roleService) {
            this.userGateway = userGateway;
            this.roleService = roleService;
        }

        public async override Task Execute(AddRoleToUserCommand input, User? user) {
            var userToRemoveFrom = await userGateway.GetUserById(input.UserId) ?? throw new EntityNotFoundException();
            var role = await roleService.GetById(input.RoleId);

            await roleService.RemoveRoleFromUser(role, userToRemoveFrom);
        }
    }
}
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Users.Security {
    [Authorization(Action = "remove-role", Scope = "users")]
    public class RemoveRoleFromUserHandler : ActionHandler<RemoveRoleFromUserCommand> {
        IUserRepo userRepo;
        IRoleService roleService;

        public RemoveRoleFromUserHandler(IUserRepo userRepo, IRoleService roleService) {
            this.userRepo = userRepo;
            this.roleService = roleService;
        }

        public async override Task Execute(RemoveRoleFromUserCommand input, User? user) {
            var userToAddTo = await userRepo.FindById(input.UserId) ?? throw new EntityNotFoundException();
            var role = await roleService.GetById(input.RoleId);

            await roleService.AddRoleToUser(role, userToAddTo);
        }
    }
}
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;

namespace DetailingArsenal.Application.Security {
    [Authorization(Action = "remove-role", Scope = "users")]
    public class RemoveRoleFromUserHandler : ActionHandler<RemoveRoleFromUserCommand> {
        IUserRepo userRepo;
        IRoleRepo roleRepo;

        public RemoveRoleFromUserHandler(IUserRepo userRepo, IRoleRepo roleRepo) {
            this.userRepo = userRepo;
            this.roleRepo = roleRepo;
        }

        public async override Task Execute(RemoveRoleFromUserCommand input, User? user) {
            var userToAddRoleTo = await userRepo.FindById(input.UserId);

            if (userToAddRoleTo == null) {
                throw new EntityNotFoundException();
            }

            var role = await roleRepo.FindById(input.RoleId);

            if (role == null) {
                throw new EntityNotFoundException();
            }

            await roleRepo.RemoveFromUser(userToAddRoleTo, role);
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;
using DetailingArsenal.Domain.Users.Security;

namespace DetailingArsenal.Domain.Common {
    public class GiveAdminAllPermissionsStep : SagaStep {
        IRoleRepo roleRepo;
        IPermissionRepo permissionRepo;

        public GiveAdminAllPermissionsStep(IRoleRepo roleRepo, IPermissionRepo permissionRepo) {
            this.roleRepo = roleRepo;
            this.permissionRepo = permissionRepo;
        }

        public async override Task Execute(SagaContext context) {
            var adminRole = await roleRepo.FindByName("Admin") ?? throw new EntityNotFoundException("No admin role?");
            var allPermissions = await permissionRepo.FindAll();

            if (adminRole.PermissionIds.Count < allPermissions.Count) {
                adminRole.PermissionIds = allPermissions.Select(p => p.Id).ToList();

                await roleRepo.Update(adminRole);
            }

        }
    }
}
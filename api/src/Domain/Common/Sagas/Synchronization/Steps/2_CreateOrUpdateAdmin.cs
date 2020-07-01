using System.Threading.Tasks;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Users;
using Serilog;

namespace DetailingArsenal.Domain.Common {
    public class CreateOrUpdateAdminStep : SagaStep {
        AdminConfig config;
        IUserService userService;
        IRoleRepo roleRepo;

        public CreateOrUpdateAdminStep(AdminConfig config, IUserService userService, IRoleRepo roleRepo) {
            this.config = config;
            this.userService = userService;
            this.roleRepo = roleRepo;
        }

        public async override Task Execute() {
            var user = await userService.GetUserByEmail(config.Email);

            if (user != null) {
                await userService.UpdatePassword(user, config.Password);
                Log.Information("Updated admin password");
            } else {
                user = await userService.CreateUser(config.Email, config.Password);

                var adminRole = await roleRepo.Find("Admin");
                await roleRepo.AddToUser(user, adminRole!);

                Log.Information("Created new admin user");
            }
        }

#pragma warning disable 1998
        public async override Task Compensate() {
            // throw new System.NotImplementedException();
        }
#pragma warning restore 1998
    }
}
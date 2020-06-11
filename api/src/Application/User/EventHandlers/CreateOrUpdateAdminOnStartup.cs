using System;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using Serilog;

namespace DetailingArsenal.Application {
    /// <summary>
    /// When a new user is generated, go ahead and email Ed.
    /// </summary>
    public class CreateOrUpdateAdminOnStartup : IBusEventHandler<StartupEvent> {
        private AdminConfig config;
        private IUserService userService;
        private IRoleRepo roleRepo;

        public CreateOrUpdateAdminOnStartup(AdminConfig config, IUserService userService, IRoleRepo roleRepo) {
            this.config = config;
            this.userService = userService;
            this.roleRepo = roleRepo;
        }

        public async Task Handle(StartupEvent busEvent) {
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
    }
}
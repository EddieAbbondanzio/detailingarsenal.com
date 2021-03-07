using System.Threading.Tasks;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;
using Serilog;

namespace DetailingArsenal.Domain {
    [DependencyInjection]
    public class CreateOrUpdateAdminStep : SagaStep {
        AdminConfig config;
        IRoleRepo roleRepo;
        IUserGateway userGateway;
        IUserRepo userRepo;

        public CreateOrUpdateAdminStep(AdminConfig config, IRoleRepo roleRepo, IUserGateway userGateway, IUserRepo userRepo) {
            this.config = config;
            this.roleRepo = roleRepo;
            this.userGateway = userGateway;
            this.userRepo = userRepo;
        }

        public async override Task Execute(SagaContext context) {
            var user = await userRepo.FindByEmail(config.Email);

            if (user != null) {
                await userGateway.UpdatePassword(user, config.Password);
                Log.Information("Updated admin password");
            } else {
                user = await userGateway.CreateUser(config.Email, config.Password);
                await userRepo.Add(user);

                var adminRole = await roleRepo.FindByName("Admin");
                await roleRepo.AddToUser(user, adminRole!);

                Log.Information("Created new admin user");
            }
        }
    }
}
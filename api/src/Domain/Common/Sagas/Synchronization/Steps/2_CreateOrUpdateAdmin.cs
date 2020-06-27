using System.Threading.Tasks;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Users;
using Serilog;

namespace DetailingArsenal.Domain.Common {
    public class CreateOrUpdateAdmin : SagaStep {
        AdminConfig config;
        IUserGateway userGateway;
        IRoleRepo roleRepo;

        public CreateOrUpdateAdmin(AdminConfig config, IUserGateway userGateway, IRoleRepo roleRepo) {
            this.config = config;
            this.userGateway = userGateway;
            this.roleRepo = roleRepo;
        }

        public async override Task Execute() {
            var user = await userGateway.GetUserByEmail(config.Email);

            if (user != null) {
                await userGateway.UpdatePassword(user, config.Password);
                Log.Information("Updated admin password");
            } else {
                user = await userGateway.CreateUser(config.Email, config.Password);

                var adminRole = await roleRepo.Find("Admin");
                await roleRepo.AddToUser(user, adminRole!);

                Log.Information("Created new admin user");
            }
        }

        public override Task Compensate() {
            throw new System.NotImplementedException();
        }
    }
}
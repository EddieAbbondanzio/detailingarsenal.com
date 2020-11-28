using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users.Security;

namespace DetailingArsenal.Domain.Common {
    public class CreateRolesStep : SagaStep {
        IRoleRepo repo;

        public CreateRolesStep(IRoleRepo repo) {
            this.repo = repo;
        }

        public async override Task Execute(SagaContext context) {
            var proRole = await repo.FindByName("Pro");

            if (proRole == null) {
                await repo.Add(new Role("Pro"));
            }

            var expiredRole = await repo.FindByName("Expired");

            if (expiredRole == null) {
                await repo.Add(new Role("Expired"));
            }
        }
    }
}
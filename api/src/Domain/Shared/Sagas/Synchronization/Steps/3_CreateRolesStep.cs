using System.Collections.Generic;
using System.Threading.Tasks;
using DetailingArsenal.Domain.Users.Security;

namespace DetailingArsenal.Domain {
    [DependencyInjection]
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

            var expiredRole = await repo.FindByName("Free");

            if (expiredRole == null) {
                await repo.Add(new Role("Free"));
            }
        }
    }
}
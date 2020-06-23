using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;

namespace DetailingArsenal.Application.Security {
    [Authorization(Action = "delete", Scope = "roles")]
    public class DeleteRoleHandler : ActionHandler<DeleteRoleCommand> {
        IRoleService service;

        public DeleteRoleHandler(IRoleService service) {
            this.service = service;
        }

        public async override Task Execute(DeleteRoleCommand input, User? user) {
            var r = await service.GetById(input.Id);
            await service.Delete(r);
        }
    }
}
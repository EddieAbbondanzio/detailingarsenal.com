using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Security;

namespace DetailingArsenal.Application.Security {
    [Authorization(Action = "delete", Scope = "permissions")]
    public class DeletePermissionHandler : ActionHandler<DeletePermissionCommand> {
        IPermissionService service;

        public DeletePermissionHandler(IPermissionService service) {
            this.service = service;
        }

        public async override Task Execute(DeletePermissionCommand input, User? user) {
            var p = await service.GetById(input.Id);
            await service.Delete(p);
        }
    }
}
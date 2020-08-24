using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "delete", Scope = "pad-series")]
    public class DeletePadSeriesHandler : ActionHandler<DeletePadSeriesCommand> {
        IPadSeriesService service;
        IMapper mapper;

        public DeletePadSeriesHandler(IPadSeriesService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task Execute(DeletePadSeriesCommand input, User? user) {
            var ps = await service.GetById(input.Id);

            await service.Delete(
                ps, user!
            );
        }
    }
}
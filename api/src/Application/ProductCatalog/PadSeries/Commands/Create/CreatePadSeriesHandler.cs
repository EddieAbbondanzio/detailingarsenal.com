using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "create", Scope = "pad-series")]
    public class CreatePadSeriesHandler : ActionHandler<CreatePadSeriesCommand, PadSeriesReadModel> {
        IPadSeriesService service;
        IMapper mapper;

        public CreatePadSeriesHandler(IPadSeriesService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<PadSeriesReadModel> Execute(CreatePadSeriesCommand input, User? user) {
            var pads = new List<PadCreateOrUpdate>();

            foreach (var pad in input.Pads) {
                pads.Add(new PadCreateOrUpdate(
                        pad.Name, PadCategoryUtils.Parse(pad.Category), pad.Image
                    )
                );
            }


            var series = await service.Create(
                new PadSeriesCreate(
                    input.Name, input.BrandId, pads
                ), user!
            );

            return mapper.Map<PadSeries, PadSeriesReadModel>(series);
        }
    }
}
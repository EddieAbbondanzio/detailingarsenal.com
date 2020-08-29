using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "create", Scope = "pad-series")]
    public class PadSeriesCreateHandler : ActionHandler<PadSeriesCreateCommand, PadSeriesReadModel> {
        IPadSeriesService service;
        IMapper mapper;

        public PadSeriesCreateHandler(IPadSeriesService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<PadSeriesReadModel> Execute(PadSeriesCreateCommand input, User? user) {
            var pads = new List<PadCreateOrUpdate>();

            foreach (var pad in input.Pads) {
                pads.Add(new PadCreateOrUpdate(
                        pad.Name, PadCategoryUtils.Parse(pad.Category), pad.Image?.ToBinaryImage()
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
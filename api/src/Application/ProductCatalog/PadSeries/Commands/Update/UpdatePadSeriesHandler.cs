using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.ProductCatalog {
    [Authorization(Action = "update", Scope = "pad-series")]
    public class UpdatePadSeriesHandler : ActionHandler<UpdatePadSeriesCommand, PadSeriesReadModel> {
        IPadSeriesService service;
        IMapper mapper;

        public UpdatePadSeriesHandler(IPadSeriesService service, IMapper mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public async override Task<PadSeriesReadModel> Execute(UpdatePadSeriesCommand input, User? user) {
            var existing = await service.GetById(input.Id);

            var pads = new List<PadCreateOrUpdate>();

            foreach (var pad in input.Pads) {
                pads.Add(new PadCreateOrUpdate(
                        pad.Name, PadCategoryUtils.Parse(pad.Category), pad.Image?.ToBinaryImage()
                    )
                );
            }


            var series = await service.Update(
                existing,
                new PadSeriesUpdate(
                    input.Id,
                    input.Name,
                     input.BrandId,
                     pads
                ), user!
            );

            return mapper.Map<PadSeries, PadSeriesReadModel>(series);
        }
    }
}
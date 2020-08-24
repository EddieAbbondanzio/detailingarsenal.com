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

            var pads = new List<PadUpdate>();

            foreach (var pad in input.Pads) {
                byte[]? image = null;

                if (pad.Image != null) {
                    image = Convert.FromBase64String(pad.Image);
                }

                pads.Add(new PadUpdate(
                        pad.Name, pad.Category, image
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Shared {
    public class GetImageByIdHandler : ActionHandler<GetImageByIdQuery, SerializedImage?> {
        IImageReader reader;

        public GetImageByIdHandler(IImageReader reader) {
            this.reader = reader;
        }

        public async override Task<SerializedImage?> Execute(GetImageByIdQuery input, User? user) {
            var image = await reader.ReadImageById(input.Id);
            return image;
        }
    }
}
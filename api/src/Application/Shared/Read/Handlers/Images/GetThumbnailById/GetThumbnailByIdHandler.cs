using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Shared {
    [DependencyInjection]
    public class GetThumbnailByIdHandler : ActionHandler<GetThumbnailByIdQuery, SerializedImage?> {
        IImageReader reader;

        public GetThumbnailByIdHandler(IImageReader reader) {
            this.reader = reader;
        }

        public async override Task<SerializedImage?> Execute(GetThumbnailByIdQuery input, User? user) {
            var image = await reader.ReadThumbnailById(input.Id);
            return image;
        }
    }
}
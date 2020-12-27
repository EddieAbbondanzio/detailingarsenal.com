using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using DetailingArsenal.Application.Shared;

namespace DetailingArsenal.Persistence.Shared {
    public class ImageReader : DatabaseInteractor, IImageReader {
        public ImageReader(IDatabase database) : base(database) { }

        public async Task<SerializedImage?> ReadImageById(Guid id) {
            throw new NotImplementedException();
        }

        public async Task<SerializedImage?> ReadThumbnailById(Guid id) {
            throw new NotImplementedException();
        }
    }
}
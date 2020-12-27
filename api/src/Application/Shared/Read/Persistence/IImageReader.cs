using System;
using System.Threading.Tasks;

namespace DetailingArsenal.Application.Shared {
    public interface IImageReader {
        Task<SerializedImage?> ReadImageById(Guid id);
        Task<SerializedImage?> ReadThumbnailById(Guid id);
    }
}
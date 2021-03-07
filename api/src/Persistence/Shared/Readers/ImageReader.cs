using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using DetailingArsenal.Domain;
using DetailingArsenal.Application.Shared;

namespace DetailingArsenal.Persistence.Shared {
    [DependencyInjection(RegisterAs = typeof(IImageReader))]
    public class ImageReader : DatabaseInteractor, IImageReader {
        public ImageReader(IDatabase database) : base(database) { }

        public async Task<SerializedImage?> ReadImageById(Guid id) {
            using (var conn = OpenConnection()) {
                var row = await conn.QueryFirstOrDefaultAsync<ImageRow>(
                    @"select image_data, mime_Type from images where id = @Id;", new { Id = id }
                );

                return new SerializedImage(row.ImageData, row.MimeType);
            }
        }

        public async Task<SerializedImage?> ReadThumbnailById(Guid id) {
            using (var conn = OpenConnection()) {
                var row = await conn.QueryFirstOrDefaultAsync<ImageRow>(
                    @"select thumbnail_data, mime_type from images where id = @Id;", new { Id = id }
                );

                return new SerializedImage(row.ThumbnailData, row.MimeType);
            }
        }
    }
}
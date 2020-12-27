using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DetailingArsenal.Application;
using DetailingArsenal.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DetailingArsenal.Application.Settings;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Application.ProductCatalog;
using System.Drawing;
using System.IO;
using DetailingArsenal.Application.Shared;

namespace DetailingArsenal.Api.Shared {
    [ApiController]
    [Route("image")]
    public class ImageController : ControllerBase {
        IMediator mediator;

        public ImageController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(Guid id) {
            var image = await mediator.Dispatch<GetImageByIdQuery, SerializedImage?>(new(id));

            if (image == null) {
                return NotFound();
            }

            return File(image.Data, image.MimeType);
        }

        [HttpGet("{id}/thumb")]
        public async Task<IActionResult> GetThumbnail(Guid id) {
            var image = await mediator.Dispatch<GetThumbnailByIdQuery, SerializedImage?>(new(id));

            if (image == null) {
                return NotFound();
            }

            return File(image.Data, image.MimeType);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Application.ProductCatalog {
    public record GetPadsFilteredQuery : IAction {
        public List<Guid> Brands { get; }
        public List<Guid> Series { get; }
        public List<PadCategory> Categories { get; }
        public List<PadMaterial> Materials { get; }
        public List<PadTexture> Textures { get; }
        public List<PolisherType> PolisherTypes { get; }
        public List<bool> HasCenterHole { get; }
        public List<int> Stars { get; }

        [JsonConstructor]
        public GetPadsFilteredQuery(List<Guid> brands, List<Guid> series, List<PadCategory> categories, List<PadMaterial> materials, List<PadTexture> textures, List<PolisherType> polisherTypes, List<bool> hasCenterHole, List<int> stars) {
            Brands = brands;
            Series = series;
            Categories = categories;
            Materials = materials;
            Textures = textures;
            PolisherTypes = polisherTypes;
            HasCenterHole = hasCenterHole;
            Stars = stars;
        }
}
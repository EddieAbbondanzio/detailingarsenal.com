using System.Collections.Generic;
using System.Text.Json.Serialization;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Application.ProductCatalog {
    public record GetAllPadSeriesQuery : IAction {
        /// <summary>
        /// Ids of brands to include.
        /// </summary>
        public string[]? Brands { get; }

        /// <summary>
        /// Ids of series to include.
        /// </summary>
        public string[]? Series { get; }

        /// <summary>
        /// Page size, page number
        /// </summary>
        public PagingOptions Paging { get; }

        public GetAllPadSeriesQuery() {
            Paging = new PagingOptions(0, 25);
        }

        [JsonConstructor]
        public GetAllPadSeriesQuery(string[]? brands = null, string[]? series = null, PagingOptions? paging = null) {
            Brands = brands;
            Series = series;
            Paging = paging ?? new PagingOptions(0, 25);
        }
    }
}
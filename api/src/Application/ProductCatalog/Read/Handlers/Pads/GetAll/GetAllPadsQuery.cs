using System;
using System.Text.Json.Serialization;

namespace DetailingArsenal.Application.ProductCatalog {
    public record GetAllPadsQuery : IAction {
        public static readonly PagingOptions DefaultPaging = new PagingOptions(0, 25);
        /// <summary>
        /// Ids of brands to include.
        /// </summary>
        public Guid[]? Brands { get; }

        /// <summary>
        /// Ids of series to include.
        /// </summary>
        public Guid[]? Series { get; }

        /// <summary>
        /// Page size, page number
        /// </summary>
        public PagingOptions Paging { get; }

        public GetAllPadsQuery() {
            Paging = DefaultPaging;
        }

        [JsonConstructor]
        public GetAllPadsQuery(Guid[]? brands = null, Guid[]? series = null, PagingOptions? paging = null) {
            Brands = brands;
            Series = series;
            Paging = paging ?? DefaultPaging;
        }
    }
}
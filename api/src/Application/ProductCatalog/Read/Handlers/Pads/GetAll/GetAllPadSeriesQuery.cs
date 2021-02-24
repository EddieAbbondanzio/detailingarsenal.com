using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Application.ProductCatalog {
    public record GetAllPadSeriesQuery : IAction {
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

        public GetAllPadSeriesQuery() {
            Paging = DefaultPaging;
        }

        [JsonConstructor]
        public GetAllPadSeriesQuery(Guid[]? brands = null, Guid[]? series = null, PagingOptions? paging = null) {
            Brands = brands;
            Series = series;
            Paging = paging ?? DefaultPaging;
        }
    }
}
using System.Collections.Generic;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Application.ProductCatalog {
    public record GetAllPadSeriesQuery : IAction {
        /// <summary>
        /// Ids of brands to include.
        /// </summary>
        public List<string>? Brands { get; }

        /// <summary>
        /// Ids of series to include.
        /// </summary>
        public List<string>? Series { get; }

        /// <summary>
        /// Page size, page number
        /// </summary>
        public PagingOptions Paging { get; }
        public GetAllPadSeriesQuery(List<string>? brands = null, List<string>? series = null, PagingOptions? paging = null) {
            Brands = brands;
            Series = series;
            Paging = paging ?? new PagingOptions(0, 25);
        }
    }
}
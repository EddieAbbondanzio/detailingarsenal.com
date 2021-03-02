using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DetailingArsenal.Domain.ProductCatalog;

namespace DetailingArsenal.Application.ProductCatalog {
    public record GetAllPadSeriesQuery : IAction {
        public static readonly PagingOptions DefaultPaging = new PagingOptions(0, 25);

        public PagingOptions Paging { get; }

        public GetAllPadSeriesQuery() {
            Paging = DefaultPaging;
        }

        public GetAllPadSeriesQuery(PagingOptions? paging = null) {
            Paging = paging ?? DefaultPaging;
        }
    }
}
using Coaching.Core.DTO.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Coaching.Core.Helpers
{
    public static class ServiceHelper
    {
        public static void ValidarPaginaLimite(ref int? page, ref int? limit)
        {
            var _page = page is null || page <= 0 ? 1 : page.Value;
            var _limit = limit is null || limit <= 0 ? 100 : limit.Value;
            _limit = Math.Min(100, _limit);

            page = _page;
            limit = _limit;
        }

        public static CollectionResponse<ResponseType> PaginarColeccion<DataType, ResponseType>(
           HttpRequest request, int? page, int? limit, IQueryable<DataType> query,
           Func<IEnumerable<DataType>, IEnumerable<ResponseType>> responseFunction)
        {
            ValidarPaginaLimite(ref page, ref limit);
            var _page = page.Value;
            var _limit = limit.Value;

            var pagedCollection = query.ToPagedList(_page, _limit);

            return PaginarColeccion(request, _page, _limit, pagedCollection, pagedCollection.HasPreviousPage,
                pagedCollection.HasNextPage, query.Count(), pagedCollection.PageCount, responseFunction);
        }

        public static CollectionResponse<ResponseType> PaginarColeccion<DataType, ResponseType>(
           HttpRequest request, int page, int limit, IEnumerable<DataType> collection,
           bool hasPreviousPage, bool hasNextPage, int totalItems, int totalPages,
           Func<IEnumerable<DataType>, IEnumerable<ResponseType>> responseFunction)
        {
            var baseUrl = $"{request.Scheme}://{request.Host}{request.Path}".ToLower();
            var linkBaseUrl = $"{baseUrl}{request.QueryString}".ToLower();
            var pageRequest = $"page={page}";
            var limitRequest = $"limit={limit}";

            var links = new[]
            {
                new LinkResponse
                {
                    Rel = "self",
                    Href = $"{linkBaseUrl}"
                },
                new LinkResponse
                {
                    Rel = "prev",
                    Href = hasPreviousPage ? linkBaseUrl.Replace(pageRequest,$"page={page-1}").Replace(limitRequest,$"limit={limit}") : ""
                },
                new LinkResponse
                {
                    Rel = "next",
                    Href = hasNextPage ? linkBaseUrl.Replace(pageRequest,$"page={page+1}").Replace(limitRequest,$"limit={limit}") : ""
                }
            };

            return new CollectionResponse<ResponseType>
            {
                CurrentPage = page,
                ItemsOnPage = Math.Min(limit, collection.Count()),

                TotalItems = totalItems,
                TotalPages = totalPages,

                HasMoreItems = hasNextPage,

                Links = links,
                Data = responseFunction(collection)
            };
        }
    }
}

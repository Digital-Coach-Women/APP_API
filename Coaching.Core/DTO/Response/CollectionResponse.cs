using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coaching.Core.DTO.Response
{
    public class CollectionResponse<T>
    {
        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; } = 1;

        [JsonPropertyName("items_on_page")]
        public int ItemsOnPage { get; set; } = 0;

        [JsonPropertyName("total_items")]
        public int TotalItems { get; set; } = 0;

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; } = 1;

        [JsonPropertyName("has_more_items")]
        public bool HasMoreItems { get; set; } = false;

        [JsonPropertyName("links")]
        public IEnumerable<LinkResponse> Links { get; set; }

        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; set; }
    }
}

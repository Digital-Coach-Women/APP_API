using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coaching.Core.DTO.Response
{
    public class DefaultResponse<T>
    {
        [JsonPropertyName("status_code")]
        public int? StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("error")]
        public bool Error { get; set; } = false;

        [JsonPropertyName("result")]
        public T Result { get; set; }
    }
}

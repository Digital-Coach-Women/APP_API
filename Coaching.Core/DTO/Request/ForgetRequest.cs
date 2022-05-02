using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coaching.Core.DTO.Request
{
    public class ForgetRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

    }
}

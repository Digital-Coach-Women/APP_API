using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coaching.Core.DTO.Request
{
    public class CertificateRequest
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("company")]
        public string Company { get; set; }
        [JsonPropertyName("speciality_level_id")]
        public int SpecialityLevelId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coaching.Core.DTO.Request
{
    public class CourseLessonRequest
    {
        [JsonPropertyName("order")]
        public int Order { get; set; }
        [JsonPropertyName("is_finish")]
        public bool IsFinish { get; set; }
    }
}

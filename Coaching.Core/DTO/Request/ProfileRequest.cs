using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coaching.Core.DTO.Request
{
    public class ProfileRequest
    {

        [JsonPropertyName("names")]
        public string Names { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("mother_last_name")]
        public string MotherLastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("birthdate")]
        public string Birthdate { get; set; }

        [JsonPropertyName("linkedin")]
        public string Linkedin { get; set; }

    }
}

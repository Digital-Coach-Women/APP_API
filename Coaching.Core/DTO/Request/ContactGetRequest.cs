using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coaching.Core.DTO.Request
{
    public class ContactGetRequest : BaseGetRequest
    {
        [FromQuery(Name = "name")]
        public string? Name { get; set; }
    }
}

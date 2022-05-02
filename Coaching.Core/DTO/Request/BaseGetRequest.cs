using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coaching.Core.DTO.Request
{
    public abstract class BaseGetRequest
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; }
        [FromQuery(Name = "limit")]
        public int Limit { get; set; }
    }
}

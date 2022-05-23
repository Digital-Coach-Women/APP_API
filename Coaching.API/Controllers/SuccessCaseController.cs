using Coaching.Helper;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Coaching.Data.Core.Coaching;
using Coaching.Core.DTO.Response;
using Coaching.Core.DTO.Request;
using Coaching.Data.Core.Coaching.Entities;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Coaching.Core.Helpers;

namespace Coaching.API.Controllers
{
    [ApiController]
    [Route(ConstantHelpers.API_PREFIX + "/success-case")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class SuccessCaseController : BaseController
    {
        private CoachingContext context;

        public SuccessCaseController(CoachingContext context)
        {
            this.context = context;
        }

        private IQueryable<SuccessStoires> PrepareQuery() => context.SuccessStoires
           .AsQueryable();

        [HttpGet]
        [ProducesResponseType(typeof(DefaultResponse<CollectionResponse<StoriesResponse>>), StatusCodes.Status200OK)]
        public IActionResult GetAll([FromQuery] BaseGetRequest model)
        {
            try
            {
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var query = PrepareQuery();

                var dtos = ServiceHelper.PaginarColeccion(HttpContext.Request, model.Page, model.Limit, query,
                  pagedEntities => StoriesResponse.Builder.From(pagedEntities).BuildAll());

                return OkResult("", dtos);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }
    }
}

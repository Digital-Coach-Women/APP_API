﻿using Coaching.Core.DTO.Request;
using Coaching.Core.DTO.Response;
using Coaching.Core.Helpers;
using Coaching.Data.Core.Coaching;
using Coaching.Data.Core.Coaching.Entities;
using Coaching.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Net.Mime;

namespace Coaching.API.Controllers
{
    [ApiController]
    [Route(ConstantHelpers.API_PREFIX + "/specialities")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class SpecialityController : BaseController
    {
        private CoachingContext context;

        public SpecialityController(CoachingContext context)
        {
            this.context = context;
        }

        private IQueryable<Speciality> PrepareQuery() => context.Speciality
            .Include(x => x.SpecialityLevel)
                .ThenInclude(x => x.Course)
            .Include(x => x.SpecialityLevel)
                .ThenInclude(x => x.SpecialityLevelCertificate)
         .AsQueryable();

        [HttpGet]
        [ProducesResponseType(typeof(DefaultResponse<CollectionResponse<SpecialityResponse>>), StatusCodes.Status200OK)]
        public IActionResult GetAll([FromQuery] SpecialityGetRequest model)
        {
            try
            {
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var query = PrepareQuery();

                if (!string.IsNullOrEmpty(model.Name))
                    query = query.Where(x => x.Name.Contains(model.Name));

                var dtos = ServiceHelper.PaginarColeccion(HttpContext.Request, model.Page, model.Limit, query,
                  pagedEntities => SpecialityResponse.Builder.From(pagedEntities).BuildAll());

                return OkResult("", dtos);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(DefaultResponse<SpecialityResponse>), StatusCodes.Status200OK)]
        public IActionResult Get(int id)
        {
            try
            {
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var query = PrepareQuery().SingleOrDefault(x => x.Id == id);
                if (query is null)
                    return NotFoundResult("Especialidad no encontrado.");
                var dto = SpecialityResponse.Builder.From(query).Build();
                return OkResult("", dto);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(DefaultResponse<SpecialityResponse>), StatusCodes.Status200OK)]
        public IActionResult Put(int id, [FromForm] SpecialityRequest model)
        {
            try
            {
                var transaction = default(IDbContextTransaction);
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var speciality = PrepareQuery().SingleOrDefault(x => x.Id == id);
                if (speciality is null)
                    return NotFoundResult("producto no encontrado");

                transaction = context.Database.BeginTransaction();

                speciality.Name = model.Name;
                speciality.Image = model.Image;
                speciality.Description = model.Description;
                context.SaveChanges();
                transaction.Commit();

                var query = PrepareQuery().SingleOrDefault(x => x.Id == id);
                var dto = SpecialityResponse.Builder.From(query).Build();
                return OkResult("", dto);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(DefaultResponse<SpecialityResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromForm] SpecialityRequest model)
        {
            try
            {
                var transaction = default(IDbContextTransaction);

                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                transaction = context.Database.BeginTransaction();

                var data = new Speciality
                {
                    Name = model.Name,
                    Image = model.Image,
                    Description = model.Description,
                };

                context.Speciality.Add(data);
                context.SaveChanges();

                transaction.Commit();

                var query = PrepareQuery().SingleOrDefault(x => x.Id == data.Id);
                var dto = SpecialityResponse.Builder.From(query).Build();
                return OkResult("", dto);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

    }
}

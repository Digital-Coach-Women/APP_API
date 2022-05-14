using Coaching.Core.DTO.Request;
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
    [Route(ConstantHelpers.API_PREFIX + "/specialities/levels")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class LevelController : BaseController
    {
        private CoachingContext context;

        public LevelController(CoachingContext context)
        {
            this.context = context;
        }

        private IQueryable<UserSpecialityLevel> PrepareUserQuery() => context.UserSpecialityLevel
            .Include(x => x.SpecialityLevel)
                .ThenInclude(x => x.Speciality)
            .Include(x => x.SpecialityLevel)
                .ThenInclude(x => x.Course)
                    .ThenInclude(x => x.CourseLesson)
            .Include(x => x.SpecialityLevel)
                .ThenInclude(x => x.SpecialityLevelCertificate)
            .AsQueryable();

        private IQueryable<SpecialityLevel> PrepareQuery() => context.SpecialityLevel
            .Include(x => x.Course)
                .ThenInclude(x => x.CourseLesson)
            .Include(x => x.SpecialityLevelCertificate)
            .AsQueryable();

        [HttpGet]
        [ProducesResponseType(typeof(DefaultResponse<CollectionResponse<LevelResponse>>), StatusCodes.Status200OK)]
        public IActionResult GetAll([FromQuery] LevelGetRequest model)
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
                  pagedEntities => LevelResponse.Builder.From(pagedEntities).BuildAll());

                return OkResult("", dtos);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

        [HttpGet]
        [Route("matriculated")]
        [ProducesResponseType(typeof(DefaultResponse<CollectionResponse<LevelResponse>>), StatusCodes.Status200OK)]
        public IActionResult GetAllMatriculated([FromQuery] LevelGetRequest model)
        {
            try
            {
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var query = PrepareUserQuery().Select(x => x.SpecialityLevel);

                if (!string.IsNullOrEmpty(model.Name))
                    query = query.Where(x => x.Name.Contains(model.Name));

                var dtos = ServiceHelper.PaginarColeccion(HttpContext.Request, model.Page, model.Limit, query,
                  pagedEntities => LevelResponse.Builder.From(pagedEntities).BuildAll());

                return OkResult("", dtos);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(DefaultResponse<LevelResponse>), StatusCodes.Status200OK)]
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
                var dto = LevelResponse.Builder.From(query).Build();

               

                return OkResult("", dto);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(DefaultResponse<LevelResponse>), StatusCodes.Status200OK)]
        public IActionResult Put(int id, [FromBody] LevelRequest model)
        {
            try
            {
                var transaction = default(IDbContextTransaction);
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var level = PrepareQuery().SingleOrDefault(x => x.Id == id);
                if (level is null)
                    return NotFoundResult("nivel de especialidad no encontrado");

                transaction = context.Database.BeginTransaction();

                level.Name = model.Name;
                level.CupImage = model.Cup;
                level.Order = model.Order;
                level.SpecialityId = model.SpecialityId;
                context.SaveChanges();
                transaction.Commit();

                var query = PrepareQuery().SingleOrDefault(x => x.Id == id);
                var dto = LevelResponse.Builder.From(query).Build();
                return OkResult("", dto);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(DefaultResponse<LevelResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] LevelRequest model)
        {
            try
            {
                var transaction = default(IDbContextTransaction);

                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                transaction = context.Database.BeginTransaction();

                var data = new SpecialityLevel
                {
                    Name = model.Name,
                    CupImage = model.Cup,
                    Order = model.Order,
                    SpecialityId = model.SpecialityId,
                };

                context.SpecialityLevel.Add(data);
                context.SaveChanges();

                transaction.Commit();

                var query = PrepareQuery().SingleOrDefault(x => x.Id == data.Id);
                var dto = LevelResponse.Builder.From(query).Build();
                return OkResult("", dto);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

        [HttpPost]
        [Route("{id}/matriculated")]
        [ProducesResponseType(typeof(DefaultResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostEnrolledLevel(int id)
        {
            try
            {
                var transaction = default(IDbContextTransaction);

                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var level = PrepareQuery().FirstOrDefault(x => x.Id == id);
                if (level is null)
                    return NotFoundResult("level not found");

                transaction = context.Database.BeginTransaction();

                var userSpecialityLevel = new UserSpecialityLevel
                {
                    UserId = userId.Value,
                    SpecialityLevelId = id,
                    IsFinish = false
                };

                context.UserSpecialityLevel.Add(userSpecialityLevel);
                context.SaveChanges();

                var userCourses = new List<UserCourse>();
                var lessonCourses =new List<UserCourseLesson>();

                foreach (var course in level.Course.OrderBy(x => x.Order)) {
                    var userCourse = new UserCourse
                    {
                        CourseId = course.Id,
                        UserSpecialityLevelId = userSpecialityLevel.Id,
                        IsFinish = false,
                        Time = 0,
                        UserId = userId.Value,
                    };
                    userCourses.Add(userCourse);

                    if (level.IsBasic == false) {
                        foreach (var lesson in course.CourseLesson.OrderBy(x => x.Order)) {
                            var lessonCourse = new UserCourseLesson
                            {
                                UserCourseId = userCourse.Id,
                                IsFinish=false,
                                Order = 1,
                                UserId = userId.Value,
                            };
                            lessonCourses.Add(lessonCourse);
                        }
                    }
                }

                context.UserCourse.AddRange(userCourses);
                context.SaveChanges();
                context.UserCourseLesson.AddRange(lessonCourses);
                context.SaveChanges();

                transaction.Commit();

                return OkResult("Nivel matriculado correctamente", new {});
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }
    }
}

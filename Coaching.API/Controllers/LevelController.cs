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
            .Include(x => x.UserCourse)
            .Include(x => x.SpecialityLevel)
                .ThenInclude(x => x.Speciality)
            .Include(x => x.SpecialityLevel)
                .ThenInclude(x => x.Course)
                    .ThenInclude(x => x.CourseLesson)
            .Include(x => x.SpecialityLevel)
                .ThenInclude(x => x.SpecialityLevelCertificate)
            .AsQueryable();

        private IQueryable<UserSpecialityLevel> PreparePartnerQuery() => context.UserSpecialityLevel
           .Include(x => x.User);

        private IQueryable<SpecialityLevel> PrepareQuery() => context.SpecialityLevel
            .Include(x => x.Speciality) 
            .Include(x => x.Course)
                .ThenInclude(x => x.CourseLesson)
            .Include(x => x.SpecialityLevelCertificate)
            .AsQueryable();

        [HttpGet]
        [Route("availables")]
        [ProducesResponseType(typeof(DefaultResponse<CollectionResponse<LevelResponse>>), StatusCodes.Status200OK)]
        public IActionResult GetAllAvailable([FromQuery] LevelAvailableGetRequest model)
        {
            try
            {
                var email = model.Email?.Trim().ToLower() ?? "";
                var user = context.User.SingleOrDefault(x => x.Email.ToLower().Trim() == email);
                var dtos = new CollectionResponse<LevelResponse>
                {
                    Data = Enumerable.Empty<LevelResponse>(),
                };
                if (user is null)
                    return OkResult("", dtos);

                var levelMatriculated = PrepareUserQuery().Where(x => x.User.Email.ToLower().Trim() == email).Select(x => x.SpecialityLevelId).ToList();

                var query = PrepareQuery().Where(x => !levelMatriculated.Contains(x.Id));

                dtos = ServiceHelper.PaginarColeccion(HttpContext.Request, model.Page, model.Limit, query,
                  pagedEntities => LevelResponse.Builder.From(pagedEntities).BuildAll());

                return OkResult("", dtos);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

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
        [Route("trophies")]
        [ProducesResponseType(typeof(DefaultResponse<CollectionResponse<LevelTrophyResponse>>), StatusCodes.Status200OK)]
        public IActionResult GetAllMatriculated()
        {
            try
            {
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var query = PrepareUserQuery().Where(x => x.UserId == userId);

                var dto = new LevelTrophyResponse();
                var levelsComplete = query.Where(x => x.IsFinish == true).Select(x => x.SpecialityLevel);
                dto.TrophiesWins = LevelResponse.Builder.From(levelsComplete).BuildAll().ToArray();
                var levelsInComplete = query.Where(x => x.IsFinish == false).Select(x => x.SpecialityLevel);
                dto.TrophiesMissing = LevelResponse.Builder.From(levelsInComplete).BuildAll().ToArray();
                return OkResult("", dto);
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

                var query = PrepareUserQuery().Where(x => x.UserId == userId);
                var levels = query.Select(x => x.SpecialityLevel);

                if (!string.IsNullOrEmpty(model.Name))
                    levels = levels.Where(x => x.Name.Contains(model.Name));

                var dtos = ServiceHelper.PaginarColeccion(HttpContext.Request, model.Page, model.Limit, levels,
                  pagedEntities => LevelResponse.Builder.From(pagedEntities).BuildAll());
                foreach (var item in dtos.Data) { 
                   var history = query.First(x => x.SpecialityLevelId == item.Id);
                    item.IsFinished = history.IsFinish;
                }
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

        [HttpGet]
        [Route("{id}/partners")]
        [ProducesResponseType(typeof(DefaultResponse<UserResponse>), StatusCodes.Status200OK)]
        public IActionResult GetAllPartners(int id, [FromQuery] PartnerGetRequest model)
        {
            try
            {
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var query = PrepareUserQuery().Where(x => x.SpecialityLevelId == id).Select(x => x.User);
                if (query is null)
                    return NotFoundResult("Especialidad no encontrado.");
                var dtos = ServiceHelper.PaginarColeccion(HttpContext.Request, model.Page, model.Limit, query,
                pagedEntities => UserResponse.Builder.From(pagedEntities).BuildAll());

                return OkResult("", dtos);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }


        [HttpGet]
        [Route("{id}/matriculated")]
        [ProducesResponseType(typeof(DefaultResponse<LevelResponse>), StatusCodes.Status200OK)]
        public IActionResult GetMatriculated(int id)
        {
            try
            {
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var query = PrepareUserQuery().SingleOrDefault(x => x.SpecialityLevelId == id && x.UserId == userId);
                if (query is null)
                    return NotFoundResult("Nivel de especialidad no encontrado.");

                var level = query.SpecialityLevel;
                var historiesCourse = query.UserCourse;

                var dto = LevelResponse.Builder.From(level).Build();
                foreach (var item in dto.Courses) { 
                    var historyCourse = historiesCourse.First(x => x.CourseId == item.Id);
                    item.IsFinish = historyCourse.IsFinish;
                    if (item.IsBasic)
                        item.Time = historyCourse.Time;
                    else {
                        item.OrderLesson = historyCourse.OrderLesson;
                    }
                }

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

                foreach (var course in level.Course.OrderBy(x => x.Order)) {
                    var userCourse = new UserCourse
                    {
                        CourseId = course.Id,
                        UserSpecialityLevelId = userSpecialityLevel.Id,
                        IsFinish = false,
                        Time = 0,
                        OrderLesson = 1,
                        UserId = userId.Value,
                    };
                    userCourses.Add(userCourse);
                }

                context.UserCourse.AddRange(userCourses);
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

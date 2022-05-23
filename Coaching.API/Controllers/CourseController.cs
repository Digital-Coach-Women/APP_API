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
    [Route(ConstantHelpers.API_PREFIX + "/specialities/levels/courses")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class CourseController : BaseController
    {
        private CoachingContext context;

        public CourseController(CoachingContext context)
        {
            this.context = context;
        }
        
        private IQueryable<UserCourse> PrepareUserCourseQuery() => context.UserCourse
            .Include(x => x.Course)
                .ThenInclude(x => x.SpecialityLevel)
                    .ThenInclude(x => x.Speciality)
            .Include(x => x.UserSpecialityLevel)
                .ThenInclude(x => x.UserCourse)
            .Include(x => x.UserSpecialityLevel)
                .ThenInclude(x => x.SpecialityLevel)
            .AsQueryable();

        private IQueryable<Course> PrepareQuery() => context.Course
            .Include(x => x.SpecialityLevel)
            .AsQueryable();

        [HttpGet]
        [ProducesResponseType(typeof(DefaultResponse<CollectionResponse<CourseResponse>>), StatusCodes.Status200OK)]
        public IActionResult GetAll([FromQuery] CourseGetRequest model)
        {
            try
            {
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var query = PrepareQuery();

                if (!string.IsNullOrEmpty(model.Name))
                    query = query.Where(x => x.Title.Contains(model.Name));

                var dtos = ServiceHelper.PaginarColeccion(HttpContext.Request, model.Page, model.Limit, query,
                  pagedEntities => CourseResponse.Builder.From(pagedEntities).BuildAll());

                return OkResult("", dtos);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(DefaultResponse<CourseResponse>), StatusCodes.Status200OK)]
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
                var dto = CourseResponse.Builder.From(query).Build();
                return OkResult("", dto);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }


        [HttpGet]
        [Route("status")]
        [ProducesResponseType(typeof(DefaultResponse<CourseStatusResponse>), StatusCodes.Status200OK)]
        public IActionResult GetStatus()
        {
            try
            {
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var query = PrepareUserCourseQuery().Where(x => x.UserId == userId);
                //if (query is null)
                //    return NotFoundResult("Especialidad no encontrado.");

                var dto = new CourseStatusResponse();
                if (query.Count(x => x.IsFinish == false) == 0)
                {
                    dto.PercentComplete = 100;
                    dto.PercentIncomplete = 0;
                    dto.Courses = new List<CourseResponse>().ToArray();
                }
                else {
                    var total = query.Count();
                    dto.PercentIncomplete = (100 * query.Count(x => x.IsFinish == false)) / total;
                    dto.PercentComplete = (100 * query.Count(x => x.IsFinish == true)) / total;
                    var coursesIncomplete = query.Where(x => x.IsFinish == false).Select(x => x.Course);
                    dto.Courses = CourseResponse.Builder.From(coursesIncomplete).BuildAll().ToArray();
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
        [ProducesResponseType(typeof(DefaultResponse<CourseResponse>), StatusCodes.Status200OK)]
        public IActionResult Put(int id, [FromBody] CourseRequest model)
        {
            try
            {
                var transaction = default(IDbContextTransaction);
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var course = PrepareQuery().SingleOrDefault(x => x.Id == id);
                if (course is null)
                    return NotFoundResult("nivel de especialidad no encontrado");

                transaction = context.Database.BeginTransaction();

                course.Video = model.Video;
                course.Title = model.Title;
                course.Description = model.Description; 
                course.SpecialityLevelId = model.SpecialityLevelId;
                course.Order = model.Order;
                context.SaveChanges();
                transaction.Commit();

                var query = PrepareQuery().Single(x => x.Id == id);
                var dto = CourseResponse.Builder.From(query).Build();
                return OkResult("", dto);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

        [HttpPut]
        [Route("{id}/time-video")]
        [ProducesResponseType(typeof(DefaultResponse<object>), StatusCodes.Status200OK)]
        public IActionResult SaveTime(int id, [FromBody] CourseVideoRequest model)
        {
            try
            {
                var transaction = default(IDbContextTransaction);
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var courseHistory = PrepareUserCourseQuery().SingleOrDefault(x => x.CourseId == id && x.UserId == userId);
                if (courseHistory is null)
                    return NotFoundResult("historial de curso no encontrado");

                transaction = context.Database.BeginTransaction();

                courseHistory.Time = model.Time;
                courseHistory.IsFinish = model.IsFinish;
                context.SaveChanges();

                var level = courseHistory.UserSpecialityLevel;
                var isIncomplete = level.UserCourse.Any(x => x.IsFinish == false);
                if (isIncomplete == false) { 
                    level.IsFinish = true;
                    if (level.SpecialityLevel.Level == ConstantHelpers.Level.INTERMEDIO && user.Level == ConstantHelpers.Level.BASICO) {
                        user.Level = ConstantHelpers.Level.INTERMEDIO;
                    }
                    if (level.SpecialityLevel.Level == ConstantHelpers.Level.AVANZADO && user.Level == ConstantHelpers.Level.INTERMEDIO)
                    {
                        user.Level = ConstantHelpers.Level.AVANZADO;
                    }
                }

                context.SaveChanges();
                transaction.Commit();

                return OkResult("tiempo guardado", new { });
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

        [HttpPut]
        [Route("{id}/lesson")]
        [ProducesResponseType(typeof(DefaultResponse<object>), StatusCodes.Status200OK)]
        public IActionResult SaveLesson(int id, [FromBody] CourseLessonRequest model)
        {
            try
            {
                var transaction = default(IDbContextTransaction);
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var courseHistory = PrepareUserCourseQuery().SingleOrDefault(x => x.CourseId == id && x.UserId == userId);
                if (courseHistory is null)
                    return NotFoundResult("historial de curso no encontrado");

                transaction = context.Database.BeginTransaction();

                courseHistory.OrderLesson = model.Order;
                courseHistory.IsFinish = courseHistory.IsFinish == true ? true : model.IsFinish;
                context.SaveChanges();

                var level = courseHistory.UserSpecialityLevel;
                var isIncomplete = level.UserCourse.Any(x => x.IsFinish == false);
                if (isIncomplete == false)
                {
                    level.IsFinish = true;
                    if (level.SpecialityLevel.Level == ConstantHelpers.Level.INTERMEDIO && user.Level == ConstantHelpers.Level.BASICO)
                    {
                        user.Level = ConstantHelpers.Level.INTERMEDIO;
                    }
                    if (level.SpecialityLevel.Level == ConstantHelpers.Level.AVANZADO && user.Level == ConstantHelpers.Level.INTERMEDIO)
                    {
                        user.Level = ConstantHelpers.Level.AVANZADO;
                    }
                }
                context.SaveChanges();
                transaction.Commit();

                return OkResult("tiempo guardado", new { });
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(DefaultResponse<CourseResponse>), StatusCodes.Status200OK)]
        public IActionResult Post([FromBody] CourseRequest model)
        {
            try
            {
                var transaction = default(IDbContextTransaction);

                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                transaction = context.Database.BeginTransaction();

                var data = new Course
                {
                    Title = model.Title,
                    Description = model.Description,
                    SpecialityLevelId = model.SpecialityLevelId,
                    Video = model.Video,
                    Order = model.Order,
                };

                context.Course.Add(data);
                context.SaveChanges();

                transaction.Commit();

                var query = PrepareQuery().Single(x => x.Id == data.Id);
                var dto = CourseResponse.Builder.From(query).Build();
                return OkResult("", dto);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }
    }
}

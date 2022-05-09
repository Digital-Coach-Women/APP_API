using Coaching.Core.DTO.Response;
using Coaching.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public static string BASE_URL = "https://localhost:44388/";
        protected DefaultResponse<object> response = new DefaultResponse<object>();
        public IActionResult NotFoundResult(string message = "")
        {
            response.StatusCode = StatusCodes.Status404NotFound;
            response.Message = message;
            response.Error = true;
            return StatusCode(StatusCodes.Status200OK, response);
        }
        public IActionResult UnauthorizedResult(string message)
        {
            response.StatusCode = StatusCodes.Status401Unauthorized;
            response.Message = message;
            response.Error = true;
            return StatusCode(StatusCodes.Status200OK, response);
        }

        public IActionResult NotContentResult(string message = "")
        {
            response.StatusCode = StatusCodes.Status204NoContent;
            response.Message = message;
            response.Error = true;
            return StatusCode(StatusCodes.Status200OK, response);
        }

        public IActionResult BadRequestResult(string message = "")
        {
            response.StatusCode = StatusCodes.Status400BadRequest;
            response.Message = message;
            response.Error = true;
            return StatusCode(StatusCodes.Status200OK, response);
        }

        public IActionResult OkResult(string message = "", object data = null)
        {
            response.StatusCode = StatusCodes.Status200OK;
            response.Message = message;
            response.Result = data;
            return StatusCode(StatusCodes.Status200OK, response);
        }

        public int? GetId(HttpRequest Request)
        {
            try
            {
                int? userId = null;
                if (Request.Headers.Keys.Contains("Authorization"))
                {
                    Request.Headers.TryGetValue("Authorization", out var token);

                    if (!token.ToString().StartsWith("Bearer "))
                        return null;

                    userId = TokenHelper.GetUserId(token.ToString()[7..]);
                }
                return userId;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}

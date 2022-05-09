using Coaching.Core.DTO.Request;
using Coaching.Core.DTO.Response;
using Coaching.Data.Core.Coaching;
using Coaching.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Coaching.API.Controllers
{
    [ApiController]
    [Route(ConstantHelpers.API_PREFIX + "/firebase-cloud-message")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class FCMController : BaseController
    {

        private CoachingContext context;

        public FCMController(CoachingContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("courses")]
        [ProducesResponseType(typeof(DefaultResponse<string>), StatusCodes.Status200OK)]
        public IActionResult Course([FromBody] FCMRequest model)
        {
            try
            {
                var users = context.User.Where(x => !string.IsNullOrEmpty(x.FcmToken)).ToList();
                foreach (var user in users) {
                    FirebaseFCMHelper.SendPushNotification(user.FcmToken, model.Title, model.Message);
                }
                return OkResult("Success","Notificación enviada");
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }


        [HttpPost]
        [Route("follow")]
        [ProducesResponseType(typeof(DefaultResponse<string>), StatusCodes.Status200OK)]
        public IActionResult Follow([FromBody] FCMRequest model)
        {
            try
            {
                var users = context.User.Where(x => !string.IsNullOrEmpty(x.FcmToken)).ToList();
                foreach (var user in users)
                {
                    FirebaseFCMHelper.SendPushNotification(user.FcmToken, model.Title, model.Message);
                }
                return OkResult("Success", "Notificación enviada");
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

        [HttpPost]
        [Route("advices")]
        [ProducesResponseType(typeof(DefaultResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Advices([FromBody] FCMRequest model)
        {
            try
            {
                var users = context.User.Where(x => !string.IsNullOrEmpty(x.FcmToken)).ToList();
                foreach (var user in users)
                {
                    FirebaseFCMHelper.SendPushNotification(user.FcmToken, model.Title, model.Message);
                }
                return OkResult("Success", "Notificación enviada");
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }
    }
}

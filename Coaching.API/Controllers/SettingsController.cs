using Coaching.Helper;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Coaching.Data.Core.Coaching;
using Coaching.Core.DTO.Response;
using Coaching.Core.DTO.Request;
using Coaching.Data.Core.Coaching.Entities;
using System.Globalization;

namespace Coaching.API.Controllers
{
    [ApiController]
    [Route(ConstantHelpers.API_PREFIX + "/settings")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class SettingsController : BaseController
    {
        private CoachingContext context;

        public SettingsController(CoachingContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("profile")]
        [ProducesResponseType(typeof(DefaultResponse<UserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var dto = UserResponse.Builder.From(user).Build();

                return OkResult("Success", dto);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }


        [HttpPut]
        [Route("profile")]
        [ProducesResponseType(typeof(DefaultResponse<UserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileRequest model)
        {
            try
            {
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var otherUser = context.User.Any(x => x.Id != userId && x.Email == model.Email);
                if (otherUser)
                    return BadRequestResult("Correo registrado en otra cuenta.");

                var birthDate = DateTime.ParseExact(model.Birthdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var diffYear = DateTime.Now - birthDate;
                var year = diffYear.TotalDays / 365.25;
                if (year < 16 || year > 23)
                {
                    return BadRequestResult("Debe tener entre 16 a 23 años de edad para registrarse.");
                }

                user.Names = model.Names;
                user.LastName = model.LastName;
                user.MotherLastName = model.MotherLastName;
                user.Email = model.Email;
                if (!string.IsNullOrEmpty(model.Password)) {
                    var encryptPass = SecurityHelper.EncryptText(model.Password);
                    user.Password = encryptPass;
                }
                user.Birthdate = birthDate;
                user.Linkedin = model.Linkedin;
                context.SaveChanges();

                var dto = UserResponse.Builder.From(user).Build();
                dto.Token = TokenHelper.GenerateJwtToken(dto.Id.ToString());
                
                return OkResult("Usuario registrado correctamente", null);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }
    }
}

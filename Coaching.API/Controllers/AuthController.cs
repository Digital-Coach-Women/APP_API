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
    [Route(ConstantHelpers.API_PREFIX + "/auth")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class AuthController : BaseController
    {
        private CoachingContext context;

        public AuthController(CoachingContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(DefaultResponse<UserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                var user = context.User
                    .SingleOrDefault(x => x.Email == model.Email);
                if (user is null)
                    return UnauthorizedResult("Correo o contraseña invalido.");
                var encryptPass = SecurityHelper.EncryptText(model.Password);
                if (user.Password != encryptPass)
                    return UnauthorizedResult("Correo o contraseña invalido.");

                if (!string.IsNullOrEmpty(model.FCMToken))
                {
                    user.FcmToken = model.FCMToken;
                    context.SaveChanges();
                }

                var dto = UserResponse.Builder.From(user).Build();
                dto.Token = TokenHelper.GenerateJwtToken(dto.Id.ToString());
                dto.Uid = await FirebaseAuthHelper.GetTokenByEmail(model.Email);

                return OkResult("Success", dto);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }


        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(DefaultResponse<UserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try
            {
                var isRegister = await FirebaseAuthHelper.RegisterUser(model.Email, model.Password);
                if (!isRegister)
                    return BadRequestResult("Hubo un problema al registrar al usuario.");

                var userExist = context.User
                    .SingleOrDefault(x => x.Email == model.Email);
                if (userExist != null)
                    return UnauthorizedResult("Correo existente.");

                var birthDate = DateTime.ParseExact(model.Birthdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var diffYear = DateTime.Now - birthDate;
                var year = diffYear.TotalDays / 365.25;
                if (year < 16 && year > 23) {
                    return BadRequestResult("Debe tener entre 16 a 23 años de edad para registrarse.");
                }

                var encryptPass = SecurityHelper.EncryptText(model.Password);

                var user = new User
                {
                    Names = model.Names,
                    LastName = model.LastName,
                    MotherLastName = model.MotherLastName,
                    Email = model.Email,
                    Password = encryptPass,
                    Linkedin = model.Linkedin,
                    Birthdate = birthDate,
                };

                context.User.Add(user);
                context.SaveChanges();

                var notificationUser = new NotificationUser {
                    UserId = user.Id,
                    SendCourses = true,
                    SendAdvice = true,
                    SendFollow = true,
                };
                context.NotificationUser.Add(notificationUser);
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

        [HttpPost]
        [Route("forget")]
        [ProducesResponseType(typeof(DefaultResponse<UserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Forget([FromBody] ForgetRequest model)
        {
            try
            {
                var userExist = context.User
                    .SingleOrDefault(x => x.Email == model.Email);
                if (userExist is null)
                    return UnauthorizedResult("Correo no existente.");

                var random = new Random();
                var code = random.Next(100000, 999999);
                userExist.Token = code.ToString();
                context.SaveChanges();

                await EmailHelper._sendMail(model.Email, "Reestablecer contraseña", Resource.forgetEmail.Replace("{code}", code.ToString()));
               
                return OkResult("Correo enviado", null);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

        [HttpPost]
        [Route("reset-password")]
        [ProducesResponseType(typeof(DefaultResponse<UserResponse>), StatusCodes.Status200OK)]
        public IActionResult ResetPassword([FromBody] ResetRequest model)
        {
            try
            {
                var userExist = context.User
                    .SingleOrDefault(x => x.Token == model.Code);
                if (userExist is null)
                    return UnauthorizedResult("Codgo Vencido.");

                var encryptPass = SecurityHelper.EncryptText(model.Password);
                userExist.Password = encryptPass;
                context.SaveChanges();

                return OkResult("Contraseña Restablecida", null);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }
    }
}

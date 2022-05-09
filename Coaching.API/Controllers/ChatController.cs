using Coaching.Core.DTO.Request;
using Coaching.Core.DTO.Response;
using Coaching.Core.Helpers;
using Coaching.Data.Core.Coaching;
using Coaching.Data.Core.Coaching.Entities;
using Coaching.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace Coaching.API.Controllers
{
    [ApiController]
    [Route(ConstantHelpers.API_PREFIX + "/chats")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class ChatController : BaseController
    {
        private CoachingContext context;

        public ChatController(CoachingContext context)
        {
            this.context = context;
        }

        private IQueryable<Chat> PrepareQuery() => context.Chat
            .Include(x => x.ChatSession)
                .ThenInclude(x => x.User)
            .Include(x => x.UserId1Navigation)
            .Include(x => x.UserId2Navigation)
         .AsQueryable();

        [HttpGet]
        [Route("contacts")]
        [ProducesResponseType(typeof(DefaultResponse<ContactResponse>), StatusCodes.Status200OK)]
        public IActionResult Contacts([FromQuery] ContactGetRequest model)
        {
            try
            {
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var query = PrepareQuery().Where(x => x.UserId1 == userId || x.UserId2 == userId).AsQueryable();

                var dtos = ServiceHelper.PaginarColeccion(HttpContext.Request, model.Page, model.Limit, query,
                  pagedEntities => ContactResponse.Builder.From(pagedEntities, userId ?? 0).BuildAll());

                return OkResult("", dtos);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

        [HttpGet]
        [Route("contacts/{id}/chats")]
        [ProducesResponseType(typeof(DefaultResponse<ChatResponse>), StatusCodes.Status200OK)]
        public IActionResult Chats(int id, [FromQuery] ChatGetRequest model)
        {
            try
            {
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var chat = PrepareQuery().FirstOrDefault(x => (x.UserId1 == id && x.UserId2 == userId) || (x.UserId2 == id && x.UserId1 == userId));
                if (chat is null) {
                    chat = new Chat { 
                        UserId1 = userId.Value,
                        UserId2 = id
                    }; 
                    context.Chat.Add(chat);
                    context.SaveChanges();
                }
                
                var query = chat.ChatSession.OrderByDescending(x => x.CreatedDate).AsQueryable();

                var dtos = ServiceHelper.PaginarColeccion(HttpContext.Request, model.Page, model.Limit, query,
                  pagedEntities => ChatResponse.Builder.From(pagedEntities, userId ?? 0).BuildAll());

                return OkResult("", dtos);
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

        [HttpPost]
        [Route("contacts/{id}/chats")]
        [ProducesResponseType(typeof(DefaultResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendMessage(int id, [FromBody] ChatRequest model)
        {
            try
            {
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");

                var chat = PrepareQuery().FirstOrDefault(x => (x.UserId1 == id && x.UserId2 == userId) || (x.UserId2 == id && x.UserId1 == userId));

                var document = $"chat-{chat.Id}";
                var fullName = $"{user.Names} {user.LastName}";
                var chatSended = await FirebaseHelper.AddChat(document, userId.Value, fullName, model.Message);
                if (!chatSended)
                    return BadRequestResult("Ocurrio un error en el servicio");

                var data = new ChatSession
                {
                    UserId = userId.Value,
                    Message = model.Message,
                    CreatedDate = DateTime.Now,
                    Id = id,
                    ChatId = chat.Id,
                };

                context.ChatSession.Add(data);
                context.SaveChanges();

                return OkResult("message sended", new { });
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

        [HttpDelete]
        [Route("contacts/{id}/chats/{chatId}")]
        [ProducesResponseType(typeof(DefaultResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteMessage(int id, int chatId)
        {
            try
            {
                var userId = GetId(Request);
                var user = context.User.SingleOrDefault(x => x.Id == userId);
                if (user is null)
                    return UnauthorizedResult("unathorized");
                var data = context.ChatSession.First(x => x.Id == chatId && chatId == id);
                if (data is null)
                    return OkResult("message deleted", new { });
                context.ChatSession.Remove(data);
                context.SaveChanges();

                return OkResult("message deleted", new { });
            }
            catch (Exception e)
            {
                return BadRequestResult(e.Message);
            }
        }

    }
}

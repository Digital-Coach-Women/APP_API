using Coaching.Data.Core.Coaching.Entities;
using Coaching.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coaching.Core.DTO.Response
{
    public class ChatSessionResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("transmitter")]
        public int Transmitter { get; set; }
        [JsonPropertyName("receiver")]
        public int Receiver { get; set; }
        [JsonPropertyName("last_message")]
        public string LastMessage { get; set; }
        [JsonPropertyName("last_date")]
        public string LastDate { get; set; }

        public class Builder
        {
            private ChatSessionResponse dto;
            private List<ChatSessionResponse> collection;

            public Builder()
            {
                this.dto = new ChatSessionResponse();
                this.collection = new List<ChatSessionResponse>();
            }
            public Builder(ChatSessionResponse dto)
            {
                this.dto = dto;
                this.collection = new List<ChatSessionResponse>();
            }
            public Builder(List<ChatSessionResponse> collection)
            {
                this.dto = new ChatSessionResponse();
                this.collection = collection;
            }

            public ChatSessionResponse Build() => dto;
            public List<ChatSessionResponse> BuildAll() => collection;

            public static Builder From(Chat entity, int trasmitterId, string tipoConstructor = ConstantHelpers.CONSTRUCTOR_DTO_SINGLE)
            {
                var dto = new ChatSessionResponse();
                dto.Id = entity.Id;
                dto.Transmitter = entity.UserId1 == trasmitterId ? entity.UserId1 : entity.UserId2;
                dto.Receiver = entity.UserId1 != trasmitterId ? entity.UserId1 : entity.UserId2;
                var chatsession = entity.ChatSession;
                if (chatsession.Count > 0)
                {
                    var lastChatSession = chatsession.OrderByDescending(x => x.CreatedDate).First();
                    dto.LastMessage = lastChatSession.Message;
                    dto.LastDate = lastChatSession.CreatedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture); ;
                }
                else
                {
                    dto.LastMessage = "";
                    dto.LastDate = "";
                }
                return new Builder(dto);
            }

            public static Builder From(IEnumerable<Chat> entities, int transmitterId)
            {
                var collection = new List<ChatSessionResponse>();

                foreach (var entity in entities)
                    collection.Add(From(entity, transmitterId, ConstantHelpers.CONSTRUCTOR_DTO_LIST).Build());

                return new Builder(collection);
            }
        }
    }
}

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
    public class ChatResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("is_transmitter")]
        public Boolean IsTransmitter { get; set; }
        [JsonPropertyName("person")]
        public UserResponse Person { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("date_message")]
        public string DateMessage { get; set; }

        public class Builder
        {
            private ChatResponse dto;
            private List<ChatResponse> collection;

            public Builder()
            {
                this.dto = new ChatResponse();
                this.collection = new List<ChatResponse>();
            }
            public Builder(ChatResponse dto)
            {
                this.dto = dto;
                this.collection = new List<ChatResponse>();
            }
            public Builder(List<ChatResponse> collection)
            {
                this.dto = new ChatResponse();
                this.collection = collection;
            }

            public ChatResponse Build() => dto;
            public List<ChatResponse> BuildAll() => collection;

            public static Builder From(ChatSession entity, int trasmitterId, string tipoConstructor = ConstantHelpers.CONSTRUCTOR_DTO_SINGLE)
            {
                var dto = new ChatResponse();
                dto.Id = entity.Id;
                dto.IsTransmitter = trasmitterId == entity.UserId ? true : false;
                dto.Person = UserResponse.Builder.From(entity.User).Build();
                dto.Message = entity.Message;
                dto.DateMessage = entity.CreatedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture); ;
                return new Builder(dto);
            }

            public static Builder From(IEnumerable<ChatSession> entities, int transmitterId)
            {
                var collection = new List<ChatResponse>();

                foreach (var entity in entities)
                    collection.Add(From(entity, transmitterId, ConstantHelpers.CONSTRUCTOR_DTO_LIST).Build());

                return new Builder(collection);
            }
        }
    }
}

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
    public class ContactResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("transmitter")]
        public UserResponse Transmitter { get; set; }
        [JsonPropertyName("receiver")]
        public UserResponse Receiver { get; set; }
        [JsonPropertyName("last_message")]
        public string LastMessage { get; set; }
        [JsonPropertyName("last_date")]
        public string LastDate { get; set; }

        public class Builder
        {
            private ContactResponse dto;
            private List<ContactResponse> collection;

            public Builder()
            {
                this.dto = new ContactResponse();
                this.collection = new List<ContactResponse>();
            }
            public Builder(ContactResponse dto)
            {
                this.dto = dto;
                this.collection = new List<ContactResponse>();
            }
            public Builder(List<ContactResponse> collection)
            {
                this.dto = new ContactResponse();
                this.collection = collection;
            }

            public ContactResponse Build() => dto;
            public List<ContactResponse> BuildAll() => collection;

            public static Builder From(Chat entity, int trasmitterId, string tipoConstructor = ConstantHelpers.CONSTRUCTOR_DTO_SINGLE)
            {
                var dto = new ContactResponse();
                dto.Id = entity.Id;
                dto.Transmitter = entity.UserId1 == trasmitterId ? UserResponse.Builder.From(entity.UserId1Navigation).Build() : UserResponse.Builder.From(entity.UserId2Navigation).Build();
                dto.Receiver = entity.UserId1 != trasmitterId ? UserResponse.Builder.From(entity.UserId1Navigation).Build() : UserResponse.Builder.From(entity.UserId2Navigation).Build();
                var lastChatSession = entity.ChatSession.OrderByDescending(x => x.CreatedDate).First();
                dto.LastMessage = lastChatSession.Message;
                dto.LastDate = lastChatSession.CreatedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture); ;
                return new Builder(dto);
            }

            public static Builder From(IEnumerable<Chat> entities, int transmitterId)
            {
                var collection = new List<ContactResponse>();

                foreach (var entity in entities)
                    collection.Add(From(entity, transmitterId, ConstantHelpers.CONSTRUCTOR_DTO_LIST).Build());

                return new Builder(collection);
            }
        }
    }
}

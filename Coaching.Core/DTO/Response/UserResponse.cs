using Coaching.Data.Core.Coaching.Entities;
using Coaching.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coaching.Core.DTO.Response
{
    public class UserResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
        [JsonPropertyName("mother_last_name")]
        public string MotherLastName { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("uid")]
        public string Uid { get; set; }

        public class Builder
        {
            private UserResponse dto;
            private List<UserResponse> collection;

            public Builder()
            {
                this.dto = new UserResponse();
                this.collection = new List<UserResponse>();
            }
            public Builder(UserResponse dto)
            {
                this.dto = dto;
                this.collection = new List<UserResponse>();
            }
            public Builder(List<UserResponse> collection)
            {
                this.dto = new UserResponse();
                this.collection = collection;
            }

            public UserResponse Build() => dto;
            public List<UserResponse> BuildAll() => collection;

            public static Builder From(User entity, string tipoConstructor = ConstantHelpers.CONSTRUCTOR_DTO_SINGLE)
            {
                var dto = new UserResponse();
                dto.Id = entity.Id;
                dto.Name = entity.Names;
                dto.LastName = entity.LastName;
                dto.Email = entity.Email;
                dto.MotherLastName = entity.MotherLastName;
                return new Builder(dto);
            }

            public static Builder From(IEnumerable<User> entities)
            {
                var collection = new List<UserResponse>();

                foreach (var entity in entities)
                    collection.Add(From(entity, ConstantHelpers.CONSTRUCTOR_DTO_LIST).Build());

                return new Builder(collection);
            }
        }
    }
}

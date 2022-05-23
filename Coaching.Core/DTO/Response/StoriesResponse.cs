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
    public class StoriesResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("profession")]
        public string Profession { get; set; }
        [JsonPropertyName("age")]
        public int Age { get; set; }
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("appointment")]
        public string Appointment { get; set; }

        public class Builder
        {
            private StoriesResponse dto;
            private List<StoriesResponse> collection;

            public Builder()
            {
                this.dto = new StoriesResponse();
                this.collection = new List<StoriesResponse>();
            }
            public Builder(StoriesResponse dto)
            {
                this.dto = dto;
                this.collection = new List<StoriesResponse>();
            }
            public Builder(List<StoriesResponse> collection)
            {
                this.dto = new StoriesResponse();
                this.collection = collection;
            }

            public StoriesResponse Build() => dto;
            public List<StoriesResponse> BuildAll() => collection;

            public static Builder From(SuccessStoires entity, string tipoConstructor = ConstantHelpers.CONSTRUCTOR_DTO_SINGLE)
            {
                var dto = new StoriesResponse();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Profession = entity.Profession;
                dto.Age = entity.Age;
                dto.City = entity.City;
                dto.Appointment = entity.Appointment;
                return new Builder(dto);
            }

            public static Builder From(IEnumerable<SuccessStoires> entities)
            {
                var collection = new List<StoriesResponse>();

                foreach (var entity in entities)
                    collection.Add(From(entity, ConstantHelpers.CONSTRUCTOR_DTO_LIST).Build());

                return new Builder(collection);
            }
        }
    }
}

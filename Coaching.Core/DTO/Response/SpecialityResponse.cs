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
    public class SpecialityResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description{ get; set; }
        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("levels")]
        public LevelResponse[] Levels { get; set; }

        public class Builder
        {
            private SpecialityResponse dto;
            private List<SpecialityResponse> collection;

            public Builder()
            {
                this.dto = new SpecialityResponse();
                this.collection = new List<SpecialityResponse>();
            }
            public Builder(SpecialityResponse dto)
            {
                this.dto = dto;
                this.collection = new List<SpecialityResponse>();
            }
            public Builder(List<SpecialityResponse> collection)
            {
                this.dto = new SpecialityResponse();
                this.collection = collection;
            }

            public SpecialityResponse Build() => dto;
            public List<SpecialityResponse> BuildAll() => collection;

            public static Builder From(Speciality entity, string tipoConstructor = ConstantHelpers.CONSTRUCTOR_DTO_SINGLE)
            {
                var dto = new SpecialityResponse();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.Description = entity.Description;
                dto.Image = entity.Image;
                dto.Levels = LevelResponse.Builder.From(entity.SpecialityLevel).BuildAll().ToArray();
                return new Builder(dto);
            }

            public static Builder From(IEnumerable<Speciality> entities)
            {
                var collection = new List<SpecialityResponse>();

                foreach (var entity in entities)
                    collection.Add(From(entity, ConstantHelpers.CONSTRUCTOR_DTO_LIST).Build());

                return new Builder(collection);
            }
        }
    }
}

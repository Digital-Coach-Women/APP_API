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
    public class LevelResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("cup_image")]
        public string CupImage { get; set; }
        [JsonPropertyName("courses")]
        public CourseResponse[] Courses { get; set; }
        [JsonPropertyName("certificates")]
        public CertificateResponse[] Certificates { get; set; }
        [JsonPropertyName("order")]
        public int Order { get; set; }
        [JsonPropertyName("is_matriculated")]
        public bool IsMatriculated { get; set; }

        [JsonPropertyName("can_matriculated")]
        public bool CanMatriculated { get; set; }
        [JsonPropertyName("is_finished")]
        public bool IsFinished { get; set; }
        [JsonPropertyName("speciality")]
        public string Speciality { get; set; }
        [JsonPropertyName("image")]
        public string Image { get; set; }
        [JsonPropertyName("is_basic")]
        public bool IsBasic { get; set; }

        public class Builder
        {
            private LevelResponse dto;
            private List<LevelResponse> collection;

            public Builder()
            {
                this.dto = new LevelResponse();
                this.collection = new List<LevelResponse>();
            }
            public Builder(LevelResponse dto)
            {
                this.dto = dto;
                this.collection = new List<LevelResponse>();
            }
            public Builder(List<LevelResponse> collection)
            {
                this.dto = new LevelResponse();
                this.collection = collection;
            }

            public LevelResponse Build() => dto;
            public List<LevelResponse> BuildAll() => collection;

            public static Builder From(SpecialityLevel entity, string tipoConstructor = ConstantHelpers.CONSTRUCTOR_DTO_SINGLE)
            {
                var dto = new LevelResponse();
                dto.Id = entity.Id;
                dto.Name = entity.Name;
                dto.CupImage = entity.CupImage;
                dto.Order = entity.Order;
                dto.Speciality = entity.Speciality.Name;
                dto.Image = entity.Speciality.Image;
                dto.IsBasic = entity.IsBasic;
                dto.Courses = CourseResponse.Builder.From(entity.Course).BuildAll().ToArray();
                dto.Certificates = CertificateResponse.Builder.From(entity.SpecialityLevelCertificate).BuildAll().ToArray();
                return new Builder(dto);
            }

            public static Builder From(IEnumerable<SpecialityLevel> entities)
            {
                var collection = new List<LevelResponse>();

                foreach (var entity in entities)
                    collection.Add(From(entity, ConstantHelpers.CONSTRUCTOR_DTO_LIST).Build());

                return new Builder(collection);
            }
        }
    }
}

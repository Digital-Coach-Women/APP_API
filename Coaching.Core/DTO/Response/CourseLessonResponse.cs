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
    public class CourseLessonResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("icon")]
        public string Icon { get; set; }
        [JsonPropertyName("is_link")]
        public bool IsLink { get; set; }
        [JsonPropertyName("link")]
        public string Link { get; set; }
        [JsonPropertyName("order")]
        public int Order { get; set; }

        public class Builder
        {
            private CourseLessonResponse dto;
            private List<CourseLessonResponse> collection;

            public Builder()
            {
                this.dto = new CourseLessonResponse();
                this.collection = new List<CourseLessonResponse>();
            }
            public Builder(CourseLessonResponse dto)
            {
                this.dto = dto;
                this.collection = new List<CourseLessonResponse>();
            }
            public Builder(List<CourseLessonResponse> collection)
            {
                this.dto = new CourseLessonResponse();
                this.collection = collection;
            }

            public CourseLessonResponse Build() => dto;
            public List<CourseLessonResponse> BuildAll() => collection;

            public static Builder From(CourseLesson entity, string tipoConstructor = ConstantHelpers.CONSTRUCTOR_DTO_SINGLE)
            {
                var dto = new CourseLessonResponse();
                dto.Id = entity.Id;
                dto.Title = entity.Title;
                dto.Description = entity.Description;
                dto.IsLink= entity.IsLink;
                dto.Link = entity.Link;
                dto.Icon = entity.Icon;
                dto.Order = entity.Order;
                return new Builder(dto);
            }

            public static Builder From(IEnumerable<CourseLesson> entities)
            {
                var collection = new List<CourseLessonResponse>();

                foreach (var entity in entities)
                    collection.Add(From(entity, ConstantHelpers.CONSTRUCTOR_DTO_LIST).Build());

                return new Builder(collection);
            }
        }
    }
}

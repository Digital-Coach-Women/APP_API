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
    public class CourseResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("video")]
        public string? Video { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("order")]
        public int Order { get; set; }
        [JsonPropertyName("is_finish")]
        public bool? IsFinish { get; set; }
        [JsonPropertyName("time")]
        public int? Time { get; set; }
        [JsonPropertyName("order_lesson")]
        public int? OrderLesson { get; set; }
        [JsonPropertyName("is_basic")]
        public bool IsBasic { get; set; }
        [JsonPropertyName("lessons")]
        public CourseLessonResponse[]? Lessons { get; set; }

        public class Builder
        {
            private CourseResponse dto;
            private List<CourseResponse> collection;

            public Builder()
            {
                this.dto = new CourseResponse();
                this.collection = new List<CourseResponse>();
            }
            public Builder(CourseResponse dto)
            {
                this.dto = dto;
                this.collection = new List<CourseResponse>();
            }
            public Builder(List<CourseResponse> collection)
            {
                this.dto = new CourseResponse();
                this.collection = collection;
            }

            public CourseResponse Build() => dto;
            public List<CourseResponse> BuildAll() => collection;

            public static Builder From(Course entity, string tipoConstructor = ConstantHelpers.CONSTRUCTOR_DTO_SINGLE)
            {
                var dto = new CourseResponse();
                dto.Id = entity.Id;
                dto.Title = entity.Title;
                dto.Description = entity.Description;
                dto.Order = entity.Order;
                dto.Video = entity.Video;
                dto.IsBasic = entity.SpecialityLevel.IsBasic;
                if (!dto.IsBasic) 
                    dto.Lessons = CourseLessonResponse.Builder.From(entity.CourseLesson).BuildAll().ToArray();
                return new Builder(dto);
            }

            public static Builder From(IEnumerable<Course> entities)
            {
                var collection = new List<CourseResponse>();

                foreach (var entity in entities)
                    collection.Add(From(entity, ConstantHelpers.CONSTRUCTOR_DTO_LIST).Build());

                return new Builder(collection);
            }
        }
    }
}

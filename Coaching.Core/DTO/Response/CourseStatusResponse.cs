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
    public class CourseStatusResponse
    {
        [JsonPropertyName("courses")]
        public CourseResponse[] Courses { get; set; }
        [JsonPropertyName("percent_complete")]
        public double PercentComplete { get; set; }
        [JsonPropertyName("percent_incomplete")]
        public double PercentIncomplete { get; set; }

        public class Builder
        {
            private CourseStatusResponse dto;
            private List<CourseStatusResponse> collection;

            public Builder()
            {
                this.dto = new CourseStatusResponse();
                this.collection = new List<CourseStatusResponse>();
            }
            public Builder(CourseStatusResponse dto)
            {
                this.dto = dto;
                this.collection = new List<CourseStatusResponse>();
            }
            public Builder(List<CourseStatusResponse> collection)
            {
                this.dto = new CourseStatusResponse();
                this.collection = collection;
            }

            public CourseStatusResponse Build() => dto;
            public List<CourseStatusResponse> BuildAll() => collection;

            //public static Builder From(Course entity, int trasmitterId, string tipoConstructor = ConstantHelpers.CONSTRUCTOR_DTO_SINGLE)
            //{
            //    var dto = new CourseStatusResponse();
            //    dto.Id = entity.Id;
            //    dto.IsTransmitter = trasmitterId == entity.UserId ? true : false;
            //    dto.Person = UserResponse.Builder.From(entity.User).Build();
            //    dto.Message = entity.Message;
            //    return new Builder(dto);
            //}

            //public static Builder From(IEnumerable<ChatSession> entities, int transmitterId)
            //{
            //    var collection = new List<CourseStatusResponse>();

            //    foreach (var entity in entities)
            //        collection.Add(From(entity, transmitterId, ConstantHelpers.CONSTRUCTOR_DTO_LIST).Build());

            //    return new Builder(collection);
            //}
        }
    }
}

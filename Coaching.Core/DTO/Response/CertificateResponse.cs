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
    public class CertificateResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
        [JsonPropertyName("company")]
        public string Company { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }

        public class Builder
        {
            private CertificateResponse dto;
            private List<CertificateResponse> collection;

            public Builder()
            {
                this.dto = new CertificateResponse();
                this.collection = new List<CertificateResponse>();
            }
            public Builder(CertificateResponse dto)
            {
                this.dto = dto;
                this.collection = new List<CertificateResponse>();
            }
            public Builder(List<CertificateResponse> collection)
            {
                this.dto = new CertificateResponse();
                this.collection = collection;
            }

            public CertificateResponse Build() => dto;
            public List<CertificateResponse> BuildAll() => collection;

            public static Builder From(SpecialityLevelCertificate entity, string tipoConstructor = ConstantHelpers.CONSTRUCTOR_DTO_SINGLE)
            {
                var dto = new CertificateResponse();
                dto.Id = entity.Id;
                dto.Title = entity.Title;
                dto.Uri = entity.Uri;
                dto.Company = entity.Company;
                return new Builder(dto);
            }

            public static Builder From(IEnumerable<SpecialityLevelCertificate> entities)
            {
                var collection = new List<CertificateResponse>();

                foreach (var entity in entities)
                    collection.Add(From(entity, ConstantHelpers.CONSTRUCTOR_DTO_LIST).Build());

                return new Builder(collection);
            }
        }
    }
}

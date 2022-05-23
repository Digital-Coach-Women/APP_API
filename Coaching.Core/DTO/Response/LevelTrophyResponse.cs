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
    public class LevelTrophyResponse
    {
        [JsonPropertyName("trophies_wins")]
        public LevelResponse[] TrophiesWins { get; set; }
        [JsonPropertyName("cups_missing")]
        public LevelResponse[] TrophiesMissing { get; set; }
        
        public class Builder
        {
            private LevelTrophyResponse dto;
            private List<LevelTrophyResponse> collection;

            public Builder()
            {
                this.dto = new LevelTrophyResponse();
                this.collection = new List<LevelTrophyResponse>();
            }
            public Builder(LevelTrophyResponse dto)
            {
                this.dto = dto;
                this.collection = new List<LevelTrophyResponse>();
            }
            public Builder(List<LevelTrophyResponse> collection)
            {
                this.dto = new LevelTrophyResponse();
                this.collection = collection;
            }

            public LevelTrophyResponse Build() => dto;
            public List<LevelTrophyResponse> BuildAll() => collection;
        }
    }
}

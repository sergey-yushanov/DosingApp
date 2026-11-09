using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DosingApp.Models.AgroAssist.Dto
{
    public class UnitsListResponseDto : List<UnitDto>
    {
    }

    public class UnitDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("group_id")]
        public int GroupId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}

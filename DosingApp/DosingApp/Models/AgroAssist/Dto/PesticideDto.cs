using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DosingApp.Models.AgroAssist.Dto
{
    public class PesticidesListResponseDto : List<PesticideDto>
    {
    }

    public class PesticideDto
    {
        [JsonIgnore]
        public string AdditionalInfo { get; set; }

        [JsonIgnore]
        public List<JsonElement> Applications { get; set; } = new List<JsonElement>();

        [JsonIgnore]
        public int? BeesHazardClassId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonIgnore]
        public int? FormId { get; set; }

        [JsonIgnore]
        public int? HumanHazardClassId { get; set; }

        [JsonIgnore]
        public List<JsonElement> Ingredients { get; set; } = new List<JsonElement>();

        [JsonIgnore]
        public bool IsDefault { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public int? PesticideExtId { get; set; }

        [JsonPropertyName("pesticide_guid")]
        public Guid PesticideGuid { get; set; }

        [JsonIgnore]
        public Guid? PesticideParentGuid { get; set; }

        [JsonIgnore]
        public string Registrant { get; set; }

        [JsonIgnore]
        public string RegistrationNumber { get; set; }

        [JsonIgnore]
        public string RegistrationPeriod { get; set; }

        [JsonIgnore]
        public int? TypeId { get; set; }

        [JsonPropertyName("unit_id")]
        public int UnitId { get; set; }
    }
}

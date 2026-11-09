using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DosingApp.Models.AgroAssist.Dto
{
    public class TechOperationsListResponseDto : List<TechOperationDto>
    {

    }

    public class TechOperationDto
    {
        [JsonPropertyName("techoperation_guid")]
        public Guid TechoperationGuid { get; set; }

        [JsonIgnore]
        public string TechoperationExtId { get; set; }

        [JsonIgnore]
        public Guid TechoperationTypeGuid { get; set; }

        [JsonIgnore]
        public int? TechoperationCategoryId { get; set; }

        [JsonIgnore]
        public DateTime? PlanBeginDatetime { get; set; }

        [JsonIgnore]
        public DateTime? PlanEndDatetime { get; set; }

        [JsonIgnore]
        public DateTime? FactBeginDatetime { get; set; }

        [JsonIgnore]
        public DateTime? FactEndDatetime { get; set; }

        [JsonPropertyName("field_guid")]
        public Guid? FieldGuid { get; set; }

        [JsonPropertyName("plan_area_hectare")]
        public double? PlanAreaHectare { get; set; }

        [JsonPropertyName("fact_area_hectare")]
        public double? FactAreaHectare { get; set; }

        [JsonIgnore]
        public double? FactAreaHectareWithOverlaps { get; set; }

        [JsonIgnore]
        public double? CompletedAreaHectare { get; set; }

        [JsonPropertyName("structure_guid")]
        public Guid? StructureGuid { get; set; }

        [JsonIgnore]
        public Guid? CompanyGuid { get; set; }

        [JsonPropertyName("seq_id")]
        public int? SeqId { get; set; }

        [JsonPropertyName("season_id")]
        public int? SeasonId { get; set; }

        [JsonIgnore]
        public Guid? AuthorGuid { get; set; }

        [JsonIgnore]
        public Guid? EditorGuid { get; set; }

        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }

        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; }

        [JsonIgnore]
        public int? StatusId { get; set; }

        [JsonPropertyName("plan_inventories")]
        public List<PlanInventoryDto> PlanInventories { get; set; }

        [JsonIgnore]
        public List<TechOperationHistoryDto> History { get; set; }
    }

    public class PlanInventoryDto
    {
        [JsonPropertyName("plan_inventory_guid")]
        public Guid PlanInventoryGuid { get; set; }

        [JsonIgnore]
        public string PlanInventoryExtId { get; set; }

        [JsonPropertyName("entity_guid")]
        public Guid? EntityGuid { get; set; }

        [JsonPropertyName("entity_type_id")]
        public int? EntityTypeId { get; set; }

        [JsonPropertyName("plan_area_hectare")]
        public double? PlanAreaHectare { get; set; }

        [JsonPropertyName("total")]
        public double? Total { get; set; }

        [JsonPropertyName("unit_id")]
        public int? UnitId { get; set; }

        [JsonIgnore]
        public Guid? AuthorGuid { get; set; }

        [JsonIgnore]
        public Guid? EditorGuid { get; set; }

        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }

        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; }
    }

    public class TechOperationHistoryDto
    {
        [JsonIgnore]
        public Guid Guid { get; set; }

        [JsonIgnore]
        public Guid? ChangedUserGuid { get; set; }

        [JsonIgnore]
        public int? StatusId { get; set; }

        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }
    }
}

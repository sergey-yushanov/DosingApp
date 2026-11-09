using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DosingApp.Models.AgroAssist.Dto
{
    public class PlanWorksExportRequestDto
    {
        [JsonPropertyName("season_id")]
        public int SeasonId { get; set; }

        [JsonPropertyName("structure_guid")]
        public Guid StructureGuid { get; set; }

        [JsonPropertyName("start_datetime")]
        public DateTime StartDatetime { get; set; }

        [JsonPropertyName("end_datetime")]
        public DateTime EndDatetime { get; set; }

        [JsonPropertyName("techoperation_status_ids")]
        public List<int> TechoperationStatusIds { get; set; }

        [JsonPropertyName("techoperation_guid")]
        public Guid? TechoperationGuid { get; set; }

        [JsonPropertyName("is_approved")]
        public bool? IsApproved { get; set; }

        [JsonPropertyName("field_guid")]
        public Guid? FieldGuid { get; set; }

        [JsonPropertyName("machine_guids")]
        public List<Guid> MachineGuids { get; set; }

        [JsonPropertyName("company_guid")]
        public Guid? CompanyGuid { get; set; }
    }

    public class PlanWorksExportResponseDto : List<PlanWorkDto>
    {
    }

    public class PlanWorkDto
    {
        [JsonPropertyName("plan_work_guid")]
        public Guid PlanWorkGuid { get; set; }

        [JsonPropertyName("plan_work_ext_id")]
        public string PlanWorkExtId { get; set; }

        [JsonPropertyName("seq_id")]
        public int SeqId { get; set; }

        [JsonPropertyName("ext_pool_id")]
        public string ExtPoolId { get; set; }

        [JsonPropertyName("techoperation_type_guid")]
        public Guid TechoperationTypeGuid { get; set; }

        [JsonPropertyName("techoperation_category_id")]
        public int? TechoperationCategoryId { get; set; }

        [JsonPropertyName("techoperation_guid")]
        public Guid? TechoperationGuid { get; set; }

        [JsonPropertyName("techoperation_status_id")]
        public int? TechoperationStatusId { get; set; }

        [JsonPropertyName("field_guid")]
        public Guid? FieldGuid { get; set; }

        [JsonPropertyName("machine_guid")]
        public Guid? MachineGuid { get; set; }

        [JsonPropertyName("tool_guid")]
        public Guid? ToolGuid { get; set; }

        [JsonPropertyName("assigned_user_guid")]
        public Guid? AssignedUserGuid { get; set; }

        [JsonPropertyName("start_datetime")]
        public DateTime? StartDatetime { get; set; }

        [JsonPropertyName("end_datetime")]
        public DateTime? EndDatetime { get; set; }

        [JsonPropertyName("plan_area_hectare")]
        public double? PlanAreaHectare { get; set; }

        [JsonPropertyName("is_approved")]
        public bool? IsApproved { get; set; }

        [JsonPropertyName("author_guid")]
        public Guid? AuthorGuid { get; set; }

        [JsonPropertyName("editor_guid")]
        public Guid? EditorGuid { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}

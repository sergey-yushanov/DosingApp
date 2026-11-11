using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DosingApp.Models.AgroAssist.Dto
{
    public class WithdrawalBulkRequestDto
    {
        [JsonPropertyName("to_be_created")]
        public List<WithdrawalCreateDto> ToBeCreated { get; set; } = new List<WithdrawalCreateDto>();

        [JsonPropertyName("to_be_updated")]
        public List<WithdrawalUpdateDto> ToBeUpdated { get; set; } = new List<WithdrawalUpdateDto>();

        [JsonPropertyName("to_be_deleted")]
        public List<WithdrawalDeleteDto> ToBeDeleted { get; set; } = new List<WithdrawalDeleteDto>();
    }

    public class WithdrawalCreateDto
    {
        [JsonPropertyName("instance")]
        public int Instance { get; set; } = 1;

        [JsonPropertyName("withdrawal_ext_id")]
        public string WithdrawalExtId { get; set; }

        [JsonPropertyName("techoperation_guid")]
        public Guid TechoperationGuid { get; set; }

        [JsonPropertyName("structure_guid")]
        public Guid? StructureGuid { get; set; }

        [JsonPropertyName("field_guid")]
        public Guid? FieldGuid { get; set; }

        [JsonPropertyName("entity_guid")]
        public Guid EntityGuid { get; set; }

        [JsonPropertyName("entity_type_id")]
        public int EntityTypeId { get; set; }

        [JsonPropertyName("total")]
        public double Total { get; set; }

        [JsonPropertyName("fact_area_hectare")]
        public double FactAreaHectare { get; set; }

        [JsonPropertyName("unit_id")]
        public int UnitId { get; set; }
    }

    public class WithdrawalUpdateDto
    {
        [JsonPropertyName("instance")]
        public int Instance { get; set; } = 1;

        [JsonPropertyName("withdrawal_guid")]
        public Guid WithdrawalGuid { get; set; }

        [JsonPropertyName("withdrawal_ext_id")]
        public string WithdrawalExtId { get; set; }

        [JsonPropertyName("entity_guid")]
        public Guid EntityGuid { get; set; }

        [JsonPropertyName("entity_type_id")]
        public int EntityTypeId { get; set; }

        [JsonPropertyName("fact_area_hectare")]
        public double FactAreaHectare { get; set; }

        [JsonPropertyName("total")]
        public double Total { get; set; }

        [JsonPropertyName("unit_id")]
        public int UnitId { get; set; }
    }

    public class WithdrawalDeleteDto
    {
        [JsonPropertyName("instance")]
        public int Instance { get; set; } = 1;

        [JsonPropertyName("withdrawal_guid")]
        public Guid WithdrawalGuid { get; set; }
    }

    public class WithdrawalBulkResponseDto
    {
        [JsonPropertyName("summary")]
        public WithdrawalBulkSummaryDto Summary { get; set; } = new WithdrawalBulkSummaryDto();

        [JsonPropertyName("results")]
        public List<WithdrawalBulkResultDto> Results { get; set; } = new List<WithdrawalBulkResultDto>();
    }

    public class WithdrawalBulkSummaryDto
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("updated")]
        public int Updated { get; set; }

        [JsonPropertyName("deleted")]
        public int Deleted { get; set; }

        [JsonPropertyName("failed")]
        public int Failed { get; set; }
    }

    public class WithdrawalBulkResultDto
    {
        [JsonPropertyName("instance")]
        public int Instance { get; set; }

        /// "created" | "updated" | "deleted" | "failed"
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("data")]
        public WithdrawalBulkResultData Data { get; set; }

        /// Текст ошибки, если status = "failed" (если API присылает)
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }

    public class WithdrawalBulkResultData
    {
        [JsonPropertyName("withdrawal_guid")]
        public Guid? WithdrawalGuid { get; set; }

        [JsonPropertyName("withdrawal_ext_id")]
        public string WithdrawalExtId { get; set; }
    }

    public static class WithdrawalBulkResultStatus
    {
        public const string Created = "created";
        public const string Updated = "updated";
        public const string Deleted = "deleted";
        public const string Failed = "failed";
    }
}

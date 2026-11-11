using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DosingApp.Models.AgroAssist.Data
{
    public enum WithdrawalSyncState
    {
        Synced = 0,
        New = 1,
        Modified = 2,
        Deleted = 3
    }

    public class Withdrawal
    {
        public int WithdrawalId { get; set; }
        public Guid? WithdrawalGuid { get; set; }
        public string WithdrawalExtId { get; set; }
        public Guid TechoperationGuid { get; set; }
        public Guid? StructureGuid { get; set; }
        public Guid? FieldGuid { get; set; }
        public Guid? EntityGuid { get; set; }
        public int? EntityTypeId { get; set; }
        public double? Total { get; set; }
        public double? FactAreaHectare { get; set; }
        public int? UnitId { get; set; }
        public WithdrawalSyncState SyncState { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class Job
    {
        public int JobId { get; set; }
        
        public int? AssignmentId { get; set; }
        public virtual Assignment Assignment { get; set; }

        public bool IsAccessToChange { get; set; }
        public double? Square { get; set; }

        public string Note { get; set; }

        public int? RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }

        public string SourceType { get; set; }
        public string DestType { get; set; }

        public bool IsSourceFacility { get; set; }
        public int? SourceFacilityId { get; set; }
        public virtual Facility SourceFacility { get; set; }
        public int? SourceFacilityTankId { get; set; }
        public virtual FacilityTank SourceFacilityTank { get; set; }

        public bool IsSourceTransport { get; set; }
        public int? SourceTransportId { get; set; }
        public virtual Transport SourceTransport { get; set; }
        public int? SourceTransportTankId { get; set; }
        public virtual TransportTank SourceTransportTank { get; set; }

        public bool IsSourceApplicator { get; set; }
        public int? SourceApplicatorId { get; set; }
        public virtual Applicator SourceApplicator { get; set; }
        public int? SourceApplicatorTankId { get; set; }
        public virtual ApplicatorTank SourceApplicatorTank { get; set; }

        public bool IsDestFacility { get; set; }
        public int? DestFacilityId { get; set; }
        public virtual Facility DestFacility { get; set; }
        public int? DestFacilityTankId { get; set; }
        public virtual FacilityTank DestFacilityTank { get; set; }

        public bool IsDestTransport { get; set; }
        public int? DestTransportId { get; set; }
        public virtual Transport DestTransport { get; set; }
        public int? DestTransportTankId { get; set; }
        public virtual TransportTank DestTransportTank { get; set; }

        public bool IsDestApplicator { get; set; }
        public int? DestApplicatorId { get; set; }
        public virtual Applicator DestApplicator { get; set; }
        public int? DestApplicatorTankId { get; set; }
        public virtual ApplicatorTank DestApplicatorTank { get; set; }

        public int? AgrYearId { get; set; }
        public virtual AgrYear AgrYear { get; set; }

        public int? FieldId { get; set; }
        public virtual Field Field { get; set; }

        public double? Volume { get; set; }
        public double? VolumeRate { get; set; }
        public string Unit { get; set; }

        public string Name { get { return Assignment?.Name; } }
    }
}

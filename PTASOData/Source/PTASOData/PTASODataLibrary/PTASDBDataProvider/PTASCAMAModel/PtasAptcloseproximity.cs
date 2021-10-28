using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAptcloseproximity
    {
        public Guid PtasAptcloseproximityid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? PtasAssessmentyear { get; set; }
        public string PtasName { get; set; }
        public int? PtasProximitycode { get; set; }
        public int? Statecode { get; set; }
        public int? Statuscode { get; set; }
        public int? Timezoneruleversionnumber { get; set; }
        public int? Utcconversiontimezonecode { get; set; }
        public long? Versionnumber { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? OrganizationidValue { get; set; }
        public Guid? PtasAssessmentyearlookupidValue { get; set; }
        public Guid? PtasSalerentneighborhoodidValue { get; set; }
        public Guid? PtasSubjectneighborhoodidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual PtasYear PtasAssessmentyearlookupidValueNavigation { get; set; }
        public virtual PtasAptneighborhood PtasSalerentneighborhoodidValueNavigation { get; set; }
        public virtual PtasAptneighborhood PtasSubjectneighborhoodidValueNavigation { get; set; }
    }
}

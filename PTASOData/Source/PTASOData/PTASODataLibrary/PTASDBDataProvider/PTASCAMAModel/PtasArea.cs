using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasArea
    {
        public PtasArea()
        {
            PtasAbstractproject = new HashSet<PtasAbstractproject>();
            PtasAbstractprojectresultparcel = new HashSet<PtasAbstractprojectresultparcel>();
            PtasCurrentuseapplication = new HashSet<PtasCurrentuseapplication>();
            PtasInspectionyear = new HashSet<PtasInspectionyear>();
            PtasNeighborhood = new HashSet<PtasNeighborhood>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
            PtasSubarea = new HashSet<PtasSubarea>();
            PtasTaxrollcorrection = new HashSet<PtasTaxrollcorrection>();
        }

        public Guid PtasAreaid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAreanumber { get; set; }
        public string PtasDescription { get; set; }
        public int? PtasDistrict { get; set; }
        public string PtasName { get; set; }
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
        public Guid? PtasAppraiseridValue { get; set; }
        public Guid? PtasResidentialappraiserteamValue { get; set; }
        public Guid? PtasSeniorappraiserValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser PtasAppraiseridValueNavigation { get; set; }
        public virtual PtasResidentialappraiserteam PtasResidentialappraiserteamValueNavigation { get; set; }
        public virtual Systemuser PtasSeniorappraiserValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractproject { get; set; }
        public virtual ICollection<PtasAbstractprojectresultparcel> PtasAbstractprojectresultparcel { get; set; }
        public virtual ICollection<PtasCurrentuseapplication> PtasCurrentuseapplication { get; set; }
        public virtual ICollection<PtasInspectionyear> PtasInspectionyear { get; set; }
        public virtual ICollection<PtasNeighborhood> PtasNeighborhood { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetail { get; set; }
        public virtual ICollection<PtasSubarea> PtasSubarea { get; set; }
        public virtual ICollection<PtasTaxrollcorrection> PtasTaxrollcorrection { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasJurisdiction
    {
        public PtasJurisdiction()
        {
            PtasCurrentuseapplication = new HashSet<PtasCurrentuseapplication>();
            PtasHomeimprovement = new HashSet<PtasHomeimprovement>();
            PtasJurisdictioncontact = new HashSet<PtasJurisdictioncontact>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
            PtasPermitPtasCurrentjurisdictionValueNavigation = new HashSet<PtasPermit>();
            PtasPermitPtasIssuedbyidValueNavigation = new HashSet<PtasPermit>();
        }

        public Guid PtasJurisdictionid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAbbreviation { get; set; }
        public int? PtasAdvancenoticeforplansdays { get; set; }
        public string PtasAppraisalnotes { get; set; }
        public string PtasDescription { get; set; }
        public int? PtasId { get; set; }
        public string PtasMainphonenumber { get; set; }
        public string PtasName { get; set; }
        public string PtasNumber { get; set; }
        public string PtasPlanrequestemail { get; set; }
        public string PtasPlanrequesturl { get; set; }
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
        public Guid? PtasCommercialcontactidValue { get; set; }
        public Guid? PtasPermitwebsiteconfigidValue { get; set; }
        public Guid? PtasResidentialcontactidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual PtasJurisdictioncontact PtasCommercialcontactidValueNavigation { get; set; }
        public virtual PtasPermitwebsiteconfig PtasPermitwebsiteconfigidValueNavigation { get; set; }
        public virtual PtasJurisdictioncontact PtasResidentialcontactidValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseapplication> PtasCurrentuseapplication { get; set; }
        public virtual ICollection<PtasHomeimprovement> PtasHomeimprovement { get; set; }
        public virtual ICollection<PtasJurisdictioncontact> PtasJurisdictioncontact { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetail { get; set; }
        public virtual ICollection<PtasPermit> PtasPermitPtasCurrentjurisdictionValueNavigation { get; set; }
        public virtual ICollection<PtasPermit> PtasPermitPtasIssuedbyidValueNavigation { get; set; }
    }
}

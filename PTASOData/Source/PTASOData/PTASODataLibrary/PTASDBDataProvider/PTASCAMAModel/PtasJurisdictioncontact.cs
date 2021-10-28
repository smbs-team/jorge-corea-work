using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasJurisdictioncontact
    {
        public PtasJurisdictioncontact()
        {
            PtasJurisdictionPtasCommercialcontactidValueNavigation = new HashSet<PtasJurisdiction>();
            PtasJurisdictionPtasResidentialcontactidValueNavigation = new HashSet<PtasJurisdiction>();
        }

        public Guid PtasJurisdictioncontactid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAddress1 { get; set; }
        public string PtasAddress2 { get; set; }
        public int? PtasAppraisalcontacttype { get; set; }
        public string PtasCity { get; set; }
        public string PtasEmail { get; set; }
        public string PtasFirstname { get; set; }
        public string PtasJobtitle { get; set; }
        public string PtasLastname { get; set; }
        public string PtasName { get; set; }
        public string PtasNote { get; set; }
        public string PtasPhonenumber { get; set; }
        public string PtasState { get; set; }
        public string PtasZipcode { get; set; }
        public int? Statecode { get; set; }
        public int? Statuscode { get; set; }
        public int? Timezoneruleversionnumber { get; set; }
        public int? Utcconversiontimezonecode { get; set; }
        public long? Versionnumber { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? OwneridValue { get; set; }
        public Guid? OwningbusinessunitValue { get; set; }
        public Guid? OwningteamValue { get; set; }
        public Guid? OwninguserValue { get; set; }
        public Guid? PtasJurisdictionidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasJurisdiction PtasJurisdictionidValueNavigation { get; set; }
        public virtual ICollection<PtasJurisdiction> PtasJurisdictionPtasCommercialcontactidValueNavigation { get; set; }
        public virtual ICollection<PtasJurisdiction> PtasJurisdictionPtasResidentialcontactidValueNavigation { get; set; }
    }
}

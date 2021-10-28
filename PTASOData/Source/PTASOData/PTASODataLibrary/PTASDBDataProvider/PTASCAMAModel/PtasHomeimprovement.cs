using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasHomeimprovement
    {
        public PtasHomeimprovement()
        {
            PtasExemption = new HashSet<PtasExemption>();
            PtasFileattachmentmetadata = new HashSet<PtasFileattachmentmetadata>();
            PtasTask = new HashSet<PtasTask>();
        }

        public Guid PtasHomeimprovementid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public string Emailaddress { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public bool? PtasApplicationsignedbytaxpayer { get; set; }
        public int? PtasApplicationsource { get; set; }
        public string PtasCompositemailingaddress { get; set; }
        public DateTimeOffset? PtasConstructionbegindate { get; set; }
        public string PtasConstructionpropertyaddress { get; set; }
        public DateTimeOffset? PtasDateapplicationreceived { get; set; }
        public DateTimeOffset? PtasDatepermitissued { get; set; }
        public string PtasDescriptionoftheimprovement { get; set; }
        public string PtasEmailaddress { get; set; }
        public DateTimeOffset? PtasEstimatedcompletiondate { get; set; }
        public decimal? PtasEstimatedconstructioncost { get; set; }
        public decimal? PtasEstimatedconstructioncostBase { get; set; }
        public decimal? PtasExemptionamount { get; set; }
        public decimal? PtasExemptionamountBase { get; set; }
        public int? PtasExemptionnumber { get; set; }
        public bool? PtasHipostcardsent { get; set; }
        public string PtasName { get; set; }
        public string PtasPhonenumber { get; set; }
        public string PtasTaxpayername { get; set; }
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
        public Guid? PtasBuildingidValue { get; set; }
        public Guid? PtasExemptionbeginyearidValue { get; set; }
        public Guid? PtasExemptionendyearidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasPermitidValue { get; set; }
        public Guid? PtasPermitjurisdictionidValue { get; set; }
        public Guid? PtasPortalcontactidValue { get; set; }
        public Guid? PtasTaxaccountidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasBuildingdetail PtasBuildingidValueNavigation { get; set; }
        public virtual PtasYear PtasExemptionbeginyearidValueNavigation { get; set; }
        public virtual PtasYear PtasExemptionendyearidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasPermit PtasPermitidValueNavigation { get; set; }
        public virtual PtasJurisdiction PtasPermitjurisdictionidValueNavigation { get; set; }
        public virtual PtasPortalcontact PtasPortalcontactidValueNavigation { get; set; }
        public virtual PtasTaxaccount PtasTaxaccountidValueNavigation { get; set; }
        public virtual ICollection<PtasExemption> PtasExemption { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadata { get; set; }
        public virtual ICollection<PtasTask> PtasTask { get; set; }
    }
}

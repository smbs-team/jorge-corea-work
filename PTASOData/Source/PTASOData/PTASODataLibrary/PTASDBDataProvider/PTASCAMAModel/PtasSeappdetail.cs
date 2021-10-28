using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSeappdetail
    {
        public PtasSeappdetail()
        {
            PtasFileattachmentmetadata = new HashSet<PtasFileattachmentmetadata>();
            PtasSeappnote = new HashSet<PtasSeappnote>();
            PtasSefrozenvalue = new HashSet<PtasSefrozenvalue>();
        }

        public Guid PtasSeappdetailid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAccountnumber { get; set; }
        public int? PtasAgeasofdecember31 { get; set; }
        public int? PtasAlternatekey { get; set; }
        public bool? PtasDocannuity { get; set; }
        public bool? PtasDocbankstatement { get; set; }
        public bool? PtasDocira { get; set; }
        public bool? PtasDocirs1040 { get; set; }
        public bool? PtasDocirs1099 { get; set; }
        public string PtasDocother { get; set; }
        public bool? PtasDocpension { get; set; }
        public bool? PtasDocrx { get; set; }
        public bool? PtasDocsocialsecurity { get; set; }
        public int? PtasExemptionlevel { get; set; }
        public int? PtasExemptiontype { get; set; }
        public int? PtasIncomelevel { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public string PtasMissingdocumentlist { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasNettotalincome { get; set; }
        public decimal? PtasNettotalincomeBase { get; set; }
        public string PtasOther { get; set; }
        public bool? PtasOverrideincomelimiterror { get; set; }
        public string PtasOverrideincomelimitnote { get; set; }
        public bool? PtasOverrideminimumageerror { get; set; }
        public string PtasOverrideminimumagenote { get; set; }
        public decimal? PtasTotalexpenses { get; set; }
        public decimal? PtasTotalexpensesBase { get; set; }
        public decimal? PtasTotalincome { get; set; }
        public decimal? PtasTotalincomeBase { get; set; }
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
        public Guid? PtasAtcjobidValue { get; set; }
        public Guid? PtasCompletedbyidValue { get; set; }
        public Guid? PtasContactidValue { get; set; }
        public Guid? PtasDecisionreasonidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasRollyearidValue { get; set; }
        public Guid? PtasSeapplicationidValue { get; set; }
        public Guid? PtasYearidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasSefrozenvalue PtasAtcjobidValueNavigation { get; set; }
        public virtual Systemuser PtasCompletedbyidValueNavigation { get; set; }
        public virtual Contact PtasContactidValueNavigation { get; set; }
        public virtual PtasSeexemptionreason PtasDecisionreasonidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasYear PtasRollyearidValueNavigation { get; set; }
        public virtual PtasSeapplication PtasSeapplicationidValueNavigation { get; set; }
        public virtual PtasYear PtasYearidValueNavigation { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadata { get; set; }
        public virtual ICollection<PtasSeappnote> PtasSeappnote { get; set; }
        public virtual ICollection<PtasSefrozenvalue> PtasSefrozenvalue { get; set; }
    }
}

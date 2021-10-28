using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPermit
    {
        public PtasPermit()
        {
            PtasHomeimprovement = new HashSet<PtasHomeimprovement>();
            PtasPermitinspectionhistory = new HashSet<PtasPermitinspectionhistory>();
        }

        public Guid PtasPermitid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public bool? PtasDeactivatedviastatus { get; set; }
        public string PtasDescription { get; set; }
        public int? PtasErrorreason { get; set; }
        public string PtasIntegrationsource { get; set; }
        public DateTimeOffset? PtasIssueddate { get; set; }
        public int? PtasJurisid { get; set; }
        public DateTimeOffset? PtasLatestpermitinspectiondate { get; set; }
        public int? PtasLatestpermitinspectiontype { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public string PtasLinktopermit { get; set; }
        public string PtasMinorifcondounit { get; set; }
        public string PtasName { get; set; }
        public string PtasParcelheadername { get; set; }
        public string PtasParcelheadertext { get; set; }
        public string PtasParcelheadertext2 { get; set; }
        public int? PtasPercentcomplete { get; set; }
        public int? PtasPermitsource { get; set; }
        public int? PtasPermitstatus { get; set; }
        public int? PtasPermittype { get; set; }
        public decimal? PtasPermitvalue { get; set; }
        public decimal? PtasPermitvalueBase { get; set; }
        public DateTimeOffset? PtasPlanreadydate { get; set; }
        public int? PtasPlanrequest { get; set; }
        public DateTimeOffset? PtasPlanrequestdate { get; set; }
        public string PtasProjectaddress { get; set; }
        public int? PtasProjectdescriptionshortcut { get; set; }
        public string PtasProjectname { get; set; }
        public bool? PtasQualifiesforhiex { get; set; }
        public DateTimeOffset? PtasRevieweddate { get; set; }
        public bool? PtasShowinspectionhistory { get; set; }
        public DateTimeOffset? PtasStatusdate { get; set; }
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
        public Guid? PtasCondounitidValue { get; set; }
        public Guid? PtasCurrentjurisdictionValue { get; set; }
        public Guid? PtasIssuedateyearidValue { get; set; }
        public Guid? PtasIssuedbyidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasReviewedbyidValue { get; set; }
        public Guid? PtasStatusupdatedbyidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasParceldetail PtasCondounitidValueNavigation { get; set; }
        public virtual PtasJurisdiction PtasCurrentjurisdictionValueNavigation { get; set; }
        public virtual PtasYear PtasIssuedateyearidValueNavigation { get; set; }
        public virtual PtasJurisdiction PtasIssuedbyidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual Systemuser PtasReviewedbyidValueNavigation { get; set; }
        public virtual Systemuser PtasStatusupdatedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasHomeimprovement> PtasHomeimprovement { get; set; }
        public virtual ICollection<PtasPermitinspectionhistory> PtasPermitinspectionhistory { get; set; }
    }
}

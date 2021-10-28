using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasValuehistory
    {
        public Guid PtasValuehistoryid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAccountnumber { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public int? PtasAlternatekey { get; set; }
        public decimal? PtasAppraisedimpvalue { get; set; }
        public decimal? PtasAppraisedimpvalueBase { get; set; }
        public decimal? PtasAppraisedlandvalue { get; set; }
        public decimal? PtasAppraisedlandvalueBase { get; set; }
        public string PtasChangereason { get; set; }
        public string PtasDirectnavigation { get; set; }
        public int? PtasFlag { get; set; }
        public decimal? PtasMarketvalue { get; set; }
        public decimal? PtasMarketvalueBase { get; set; }
        public string PtasName { get; set; }
        public string PtasParcelheadername { get; set; }
        public string PtasParcelheadertext { get; set; }
        public string PtasParcelheadertext2 { get; set; }
        public int? PtasRecvisn { get; set; }
        public decimal? PtasTaxableimpvalue { get; set; }
        public decimal? PtasTaxableimpvalueBase { get; set; }
        public decimal? PtasTaxablelandvalue { get; set; }
        public decimal? PtasTaxablelandvalueBase { get; set; }
        public string PtasTaxstatus { get; set; }
        public decimal? PtasTotalappraisedvalue { get; set; }
        public decimal? PtasTotalappraisedvalueBase { get; set; }
        public decimal? PtasTotaltaxablevalue { get; set; }
        public decimal? PtasTotaltaxablevalueBase { get; set; }
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
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasYearidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasYear PtasYearidValueNavigation { get; set; }
    }
}

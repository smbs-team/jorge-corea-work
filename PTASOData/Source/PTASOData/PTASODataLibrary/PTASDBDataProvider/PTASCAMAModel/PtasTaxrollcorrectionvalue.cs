using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasTaxrollcorrectionvalue
    {
        public Guid PtasTaxrollcorrectionvalueid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public bool? PtasAdjusttaxvalue { get; set; }
        public decimal? PtasAppraisedimpfrom { get; set; }
        public decimal? PtasAppraisedimpfromBase { get; set; }
        public decimal? PtasAppraisedimpto { get; set; }
        public decimal? PtasAppraisedimptoBase { get; set; }
        public decimal? PtasAppraisedlandfrom { get; set; }
        public decimal? PtasAppraisedlandfromBase { get; set; }
        public decimal? PtasAppraisedlandto { get; set; }
        public decimal? PtasAppraisedlandtoBase { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasTaxableimpfrom { get; set; }
        public decimal? PtasTaxableimpfromBase { get; set; }
        public decimal? PtasTaxableimpto { get; set; }
        public decimal? PtasTaxableimptoBase { get; set; }
        public decimal? PtasTaxablelandfrom { get; set; }
        public decimal? PtasTaxablelandfromBase { get; set; }
        public decimal? PtasTaxablelandto { get; set; }
        public decimal? PtasTaxablelandtoBase { get; set; }
        public decimal? PtasTotalappraisedfrom { get; set; }
        public decimal? PtasTotalappraisedfromBase { get; set; }
        public decimal? PtasTotalappraisedto { get; set; }
        public decimal? PtasTotalappraisedtoBase { get; set; }
        public decimal? PtasTotaltaxablefrom { get; set; }
        public decimal? PtasTotaltaxablefromBase { get; set; }
        public decimal? PtasTotaltaxableto { get; set; }
        public decimal? PtasTotaltaxabletoBase { get; set; }
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
        public Guid? PtasLevycodeidValue { get; set; }
        public Guid? PtasParceldetailidValue { get; set; }
        public Guid? PtasTaxaccountidValue { get; set; }
        public Guid? PtasTaxrollcorrectionidValue { get; set; }
        public Guid? PtasTaxrollyearidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasLevycode PtasLevycodeidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParceldetailidValueNavigation { get; set; }
        public virtual PtasTaxaccount PtasTaxaccountidValueNavigation { get; set; }
        public virtual PtasTaxrollcorrection PtasTaxrollcorrectionidValueNavigation { get; set; }
        public virtual PtasYear PtasTaxrollyearidValueNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasRefundpetition
    {
        public Guid PtasRefundpetitionid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAccountnumbertxt { get; set; }
        public int? PtasAdjustedexemptionlevel { get; set; }
        public decimal? PtasAdjustedtaxablevalue0accountimp { get; set; }
        public decimal? PtasAdjustedtaxablevalue0accountimpBase { get; set; }
        public decimal? PtasAdjustedtaxablevalue0accountland { get; set; }
        public decimal? PtasAdjustedtaxablevalue0accountlandBase { get; set; }
        public decimal? PtasAdjustedtaxablevalue0accounttotal { get; set; }
        public decimal? PtasAdjustedtaxablevalue0accounttotalBase { get; set; }
        public decimal? PtasAdjustedtaxablevalue8accountimp { get; set; }
        public decimal? PtasAdjustedtaxablevalue8accountimpBase { get; set; }
        public decimal? PtasAdjustedtaxablevalue8accountland { get; set; }
        public decimal? PtasAdjustedtaxablevalue8accountlandBase { get; set; }
        public decimal? PtasAdjustedtaxablevalue8accounttotal { get; set; }
        public decimal? PtasAdjustedtaxablevalue8accounttotalBase { get; set; }
        public decimal? PtasAdjustedtaxamountatregularrate { get; set; }
        public decimal? PtasAdjustedtaxamountatregularrateBase { get; set; }
        public decimal? PtasAdjustedtaxamountatseniorrate { get; set; }
        public decimal? PtasAdjustedtaxamountatseniorrateBase { get; set; }
        public decimal? PtasAdjustedtaxtotal { get; set; }
        public decimal? PtasAdjustedtaxtotalBase { get; set; }
        public decimal? PtasAdjustedtotaltaxablevalue { get; set; }
        public decimal? PtasAdjustedtotaltaxablevalueBase { get; set; }
        public int? PtasCurrentexemptionlevel { get; set; }
        public decimal? PtasCurrenttaxablevalue0accountimp { get; set; }
        public decimal? PtasCurrenttaxablevalue0accountimpBase { get; set; }
        public decimal? PtasCurrenttaxablevalue0accountland { get; set; }
        public decimal? PtasCurrenttaxablevalue0accountlandBase { get; set; }
        public decimal? PtasCurrenttaxablevalue0accounttotal { get; set; }
        public decimal? PtasCurrenttaxablevalue0accounttotalBase { get; set; }
        public decimal? PtasCurrenttaxablevalue8accountimp { get; set; }
        public decimal? PtasCurrenttaxablevalue8accountimpBase { get; set; }
        public decimal? PtasCurrenttaxablevalue8accountland { get; set; }
        public decimal? PtasCurrenttaxablevalue8accountlandBase { get; set; }
        public decimal? PtasCurrenttaxablevalue8accounttotal { get; set; }
        public decimal? PtasCurrenttaxablevalue8accounttotalBase { get; set; }
        public decimal? PtasCurrenttaxamountatregularrate { get; set; }
        public decimal? PtasCurrenttaxamountatregularrateBase { get; set; }
        public decimal? PtasCurrenttaxamountatseniorrate { get; set; }
        public decimal? PtasCurrenttaxamountatseniorrateBase { get; set; }
        public decimal? PtasCurrenttaxbilled0account { get; set; }
        public decimal? PtasCurrenttaxbilled0accountBase { get; set; }
        public decimal? PtasCurrenttaxbilled8account { get; set; }
        public decimal? PtasCurrenttaxbilled8accountBase { get; set; }
        public decimal? PtasCurrenttaxbilledtotal { get; set; }
        public decimal? PtasCurrenttaxbilledtotalBase { get; set; }
        public decimal? PtasCurrenttaxpaid0account { get; set; }
        public decimal? PtasCurrenttaxpaid0accountBase { get; set; }
        public decimal? PtasCurrenttaxpaid8account { get; set; }
        public decimal? PtasCurrenttaxpaid8accountBase { get; set; }
        public decimal? PtasCurrenttaxpaidtotal { get; set; }
        public decimal? PtasCurrenttaxpaidtotalBase { get; set; }
        public decimal? PtasCurrenttaxtotal { get; set; }
        public decimal? PtasCurrenttaxtotalBase { get; set; }
        public decimal? PtasCurrenttotaltaxablevalue { get; set; }
        public decimal? PtasCurrenttotaltaxablevalueBase { get; set; }
        public DateTime? PtasFirsthalfreceiptdate { get; set; }
        public string PtasFirsthalfreceiptnumber { get; set; }
        public string PtasName { get; set; }
        public string PtasPayeename { get; set; }
        public decimal? PtasRefundamount { get; set; }
        public decimal? PtasRefundamountBase { get; set; }
        public int? PtasRefundreasoncode { get; set; }
        public decimal? PtasRegularrate { get; set; }
        public DateTimeOffset? PtasSecondhalfreceiptdate { get; set; }
        public string PtasSecondhalfreceiptnumber { get; set; }
        public decimal? PtasSecondhalfrefundamount { get; set; }
        public decimal? PtasSecondhalfrefundamountBase { get; set; }
        public decimal? PtasSeniorrate { get; set; }
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
        public Guid? PtasRefundlevyidValue { get; set; }
        public Guid? PtasSeapplicationidValue { get; set; }
        public Guid? PtasTaxaccountidValue { get; set; }
        public Guid? PtasTaxrollyearidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasRefundpetitionlevyrate PtasRefundlevyidValueNavigation { get; set; }
        public virtual PtasSeapplication PtasSeapplicationidValueNavigation { get; set; }
        public virtual PtasTaxaccount PtasTaxaccountidValueNavigation { get; set; }
        public virtual PtasYear PtasTaxrollyearidValueNavigation { get; set; }
    }
}

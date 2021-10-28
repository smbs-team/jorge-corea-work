using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCurrentusebacktaxyear
    {
        public Guid PtasCurrentusebacktaxyearid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAdditionaltaxdue { get; set; }
        public decimal? PtasAdditionaltaxdueBase { get; set; }
        public decimal? PtasCurrentusevalue { get; set; }
        public decimal? PtasCurrentusevalueBase { get; set; }
        public decimal? PtasDifference { get; set; }
        public decimal? PtasDifferenceBase { get; set; }
        public int? PtasInterestpct { get; set; }
        public decimal? PtasLevyrate { get; set; }
        public decimal? PtasMarketvalue { get; set; }
        public decimal? PtasMarketvalueBase { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasTotalinterest { get; set; }
        public decimal? PtasTotalinterestBase { get; set; }
        public decimal? PtasTotaltaxandinterest { get; set; }
        public decimal? PtasTotaltaxandinterestBase { get; set; }
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
        public Guid? PtasCurrentusebacktaxstatementidValue { get; set; }
        public Guid? PtasTaxyearidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasCurrentusebacktaxstatement PtasCurrentusebacktaxstatementidValueNavigation { get; set; }
        public virtual PtasYear PtasTaxyearidValueNavigation { get; set; }
    }
}

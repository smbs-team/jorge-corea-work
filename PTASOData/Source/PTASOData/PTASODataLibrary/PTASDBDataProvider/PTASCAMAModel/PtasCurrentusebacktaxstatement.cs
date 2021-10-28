using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCurrentusebacktaxstatement
    {
        public PtasCurrentusebacktaxstatement()
        {
            PtasCurrentusebacktaxyear = new HashSet<PtasCurrentusebacktaxyear>();
        }

        public Guid PtasCurrentusebacktaxstatementid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? Ptas20penalty { get; set; }
        public decimal? Ptas20penaltyBase { get; set; }
        public bool? PtasAdd20penalty { get; set; }
        public bool? PtasBacktaxprocessed { get; set; }
        public decimal? PtasCompensatingtax { get; set; }
        public decimal? PtasCompensatingtaxBase { get; set; }
        public decimal? PtasCurrentusevalue { get; set; }
        public decimal? PtasCurrentusevalueBase { get; set; }
        public DateTimeOffset? PtasDateofremoval { get; set; }
        public decimal? PtasForestlandvalue { get; set; }
        public decimal? PtasForestlandvalueBase { get; set; }
        public decimal? PtasGrandtotaltaxinterestandpenalty { get; set; }
        public decimal? PtasGrandtotaltaxinterestandpenaltyBase { get; set; }
        public int? PtasInterestrate { get; set; }
        public decimal? PtasLevyrate { get; set; }
        public decimal? PtasMarketvalue { get; set; }
        public decimal? PtasMarketvalueBase { get; set; }
        public string PtasName { get; set; }
        public int? PtasNumberofdaysincurrentuse { get; set; }
        public int? PtasNumberofdaysinyear { get; set; }
        public int? PtasNumberofdaysremainingafterremoval { get; set; }
        public int? PtasNumberofmonths { get; set; }
        public int? PtasNumberofyearsofbacktaxes { get; set; }
        public decimal? PtasProrationfactorremainder { get; set; }
        public decimal? PtasRemainderadditionaltax { get; set; }
        public decimal? PtasRemainderadditionaltaxBase { get; set; }
        public decimal? PtasRemainderproratedcurrentusevalue { get; set; }
        public decimal? PtasRemainderproratedcurrentusevalueBase { get; set; }
        public decimal? PtasRemainderproratedmarketvalue { get; set; }
        public decimal? PtasRemainderproratedmarketvalueBase { get; set; }
        public decimal? PtasTotaladditionaltax { get; set; }
        public decimal? PtasTotaladditionaltaxBase { get; set; }
        public decimal? PtasTotaladditionaltaxinterestandpenalty { get; set; }
        public decimal? PtasTotaladditionaltaxinterestandpenaltyBase { get; set; }
        public decimal? PtasTotalinterest { get; set; }
        public decimal? PtasTotalinterestBase { get; set; }
        public decimal? PtasTotalprioryearstaxandinterest { get; set; }
        public decimal? PtasTotalprioryearstaxandinterestBase { get; set; }
        public decimal? PtasYtdadditionaltax { get; set; }
        public decimal? PtasYtdadditionaltaxBase { get; set; }
        public decimal? PtasYtdproratedcurrentusevalue { get; set; }
        public decimal? PtasYtdproratedcurrentusevalueBase { get; set; }
        public decimal? PtasYtdproratedmarketvalue { get; set; }
        public decimal? PtasYtdproratedmarketvalueBase { get; set; }
        public decimal? PtasYtdprorationfactor { get; set; }
        public decimal? PtasYtdtaxandinterest { get; set; }
        public decimal? PtasYtdtaxandinterestBase { get; set; }
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
        public Guid? PtasCurrentuseparcelidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasCurrentuseapplicationparcel PtasCurrentuseparcelidValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentusebacktaxyear> PtasCurrentusebacktaxyear { get; set; }
    }
}

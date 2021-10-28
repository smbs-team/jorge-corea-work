using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasLevyworksheetdetails
    {
        public PtasLevyworksheetdetails()
        {
            PtasLevyordinancerequest = new HashSet<PtasLevyordinancerequest>();
            PtasRatesheetdetail = new HashSet<PtasRatesheetdetail>();
        }

        public Guid PtasLevyworksheetdetailsid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAllowablelevy { get; set; }
        public decimal? PtasAllowablelevyBase { get; set; }
        public decimal? PtasAmtrequested { get; set; }
        public decimal? PtasAmtrequestedBase { get; set; }
        public decimal? PtasAnnexassessedval { get; set; }
        public decimal? PtasAnnexassessedvalBase { get; set; }
        public decimal? PtasAnnexlevy { get; set; }
        public decimal? PtasAnnexlevyBase { get; set; }
        public decimal? PtasAnnexrate { get; set; }
        public decimal? PtasBankedcapacity { get; set; }
        public decimal? PtasBankedcapacityBase { get; set; }
        public bool? PtasCertified { get; set; }
        public decimal? PtasCertreglevyamt { get; set; }
        public decimal? PtasCertreglevyamtBase { get; set; }
        public decimal? PtasCertreglevyrate { get; set; }
        public bool? PtasConsolidatedlevy { get; set; }
        public decimal? PtasCurrentyearactualregamt { get; set; }
        public decimal? PtasCurrentyearactualregamtBase { get; set; }
        public decimal? PtasCurrentyearactualregrate { get; set; }
        public decimal? PtasCurrentyearallowlessrefund { get; set; }
        public decimal? PtasCurrentyearallowlessrefundBase { get; set; }
        public decimal? PtasCurrentyearallowlevy { get; set; }
        public decimal? PtasCurrentyearallowlevyBase { get; set; }
        public int? PtasCurrentyearallowreason { get; set; }
        public decimal? PtasDistreqdollarincrease { get; set; }
        public decimal? PtasDistreqdollarincreaseBase { get; set; }
        public decimal? PtasDistreqpercentincrease { get; set; }
        public decimal? PtasDistrictannexlevy { get; set; }
        public decimal? PtasDistrictannexlevyBase { get; set; }
        public decimal? PtasDollarincrease { get; set; }
        public decimal? PtasDollarincreaseBase { get; set; }
        public decimal? PtasExcessassessedval { get; set; }
        public decimal? PtasExcessassessedvalBase { get; set; }
        public decimal? PtasFiredistrictacuallevy { get; set; }
        public string PtasFirstyearlidliftlabel { get; set; }
        public decimal? PtasFirstyearlidlifts { get; set; }
        public decimal? PtasFirstyearlidliftsBase { get; set; }
        public decimal? PtasHighestlawlevy { get; set; }
        public decimal? PtasHighestlawlevyBase { get; set; }
        public int? PtasHighestlevyreason { get; set; }
        public decimal? PtasImplicitpricedeflator { get; set; }
        public decimal? PtasIncreasestateutilityval { get; set; }
        public decimal? PtasIncreasestateutilityvalBase { get; set; }
        public decimal? PtasIpdallowablelevy { get; set; }
        public decimal? PtasIpdallowablelevyBase { get; set; }
        public decimal? PtasIpdannexlevy { get; set; }
        public decimal? PtasIpdannexlevyBase { get; set; }
        public decimal? PtasIpdannexrate { get; set; }
        public decimal? PtasIpddollarincrease { get; set; }
        public decimal? PtasIpddollarincreaseBase { get; set; }
        public decimal? PtasIpdlevylimitfactor { get; set; }
        public decimal? PtasIpdlevylimitfactorBase { get; set; }
        public decimal? PtasIpdpercentincrease { get; set; }
        public decimal? PtasIpdreglevyrate { get; set; }
        public decimal? PtasIpdtotallimitfactor { get; set; }
        public decimal? PtasIpdtotallimitfactorBase { get; set; }
        public decimal? PtasIpdtotallimitfactorlidlifts { get; set; }
        public decimal? PtasIpdtotallimitfactorlidliftsBase { get; set; }
        public decimal? PtasIpdtotalrcw8455 { get; set; }
        public decimal? PtasIpdtotalrcw8455Base { get; set; }
        public decimal? PtasIpdtotalrcw8455refunds { get; set; }
        public decimal? PtasIpdtotalrcw8455refundsBase { get; set; }
        public decimal? PtasLastyearactuallevy { get; set; }
        public decimal? PtasLastyearactuallevyBase { get; set; }
        public decimal? PtasLastyearregrate { get; set; }
        public decimal? PtasLastyearutilityval { get; set; }
        public decimal? PtasLastyearutilityvalBase { get; set; }
        public decimal? PtasLevybasislimitfactor { get; set; }
        public decimal? PtasLevybasislimitfactorBase { get; set; }
        public bool? PtasLevycorrection { get; set; }
        public decimal? PtasLevycorrectionamt { get; set; }
        public decimal? PtasLevycorrectionamtBase { get; set; }
        public decimal? PtasLibrarydistrictactuallevy { get; set; }
        public decimal? PtasLimitfactor { get; set; }
        public decimal? PtasLocalnewcons { get; set; }
        public decimal? PtasLocalnewconsBase { get; set; }
        public decimal? PtasMaxstatutorylevy { get; set; }
        public decimal? PtasMaxstatutorylevyBase { get; set; }
        public decimal? PtasMaxstatutoryrate { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasNewconslevy { get; set; }
        public decimal? PtasNewconslevyBase { get; set; }
        public int? PtasOverridelevyamountandrate { get; set; }
        public bool? PtasOverridelimitfactor { get; set; }
        public bool? PtasOverwritefirstyearlabel { get; set; }
        public decimal? PtasPercentincrease { get; set; }
        public decimal? PtasRatebasedallowlevy { get; set; }
        public decimal? PtasRefundamt { get; set; }
        public decimal? PtasRefundamtBase { get; set; }
        public decimal? PtasRefunds { get; set; }
        public decimal? PtasRefundsBase { get; set; }
        public decimal? PtasRegassessedval { get; set; }
        public decimal? PtasRegassessedvalBase { get; set; }
        public decimal? PtasRegexpenserate { get; set; }
        public decimal? PtasRegularlevyrate { get; set; }
        public int? PtasTaxdistricttype { get; set; }
        public decimal? PtasTotalliftrate { get; set; }
        public decimal? PtasTotallimitfactorlevy { get; set; }
        public decimal? PtasTotallimitfactorlevyBase { get; set; }
        public decimal? PtasTotallimitfactorlidlifts { get; set; }
        public decimal? PtasTotallimitfactorlidliftsBase { get; set; }
        public decimal? PtasTotalnewcons { get; set; }
        public decimal? PtasTotalnewconsBase { get; set; }
        public decimal? PtasTotalrcw8455levy { get; set; }
        public decimal? PtasTotalrcw8455levyBase { get; set; }
        public decimal? PtasTotalrcw8455refunds { get; set; }
        public decimal? PtasTotalrcw8455refundsBase { get; set; }
        public decimal? PtasUtilityval { get; set; }
        public decimal? PtasUtilityvalBase { get; set; }
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
        public Guid? PtasHighestlevyyearidValue { get; set; }
        public Guid? PtasLevycorrectionyearidValue { get; set; }
        public Guid? PtasTaxdistrictidValue { get; set; }
        public Guid? PtasYearidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual ICollection<PtasLevyordinancerequest> PtasLevyordinancerequest { get; set; }
        public virtual ICollection<PtasRatesheetdetail> PtasRatesheetdetail { get; set; }
    }
}

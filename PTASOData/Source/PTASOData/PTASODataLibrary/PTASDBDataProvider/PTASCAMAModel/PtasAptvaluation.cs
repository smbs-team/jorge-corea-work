using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAptvaluation
    {
        public PtasAptvaluation()
        {
            PtasAptcommercialincomeexpense = new HashSet<PtasAptcommercialincomeexpense>();
            PtasAptcomparablerent = new HashSet<PtasAptcomparablerent>();
            PtasAptcomparablesale = new HashSet<PtasAptcomparablesale>();
        }

        public Guid PtasAptvaluationid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? PtasAirportnoise { get; set; }
        public decimal? PtasAnnuallaundryincome { get; set; }
        public decimal? PtasAnnuallaundryincomeBase { get; set; }
        public decimal? PtasAnnualmiscellaneousincome { get; set; }
        public decimal? PtasAnnualmiscellaneousincomeBase { get; set; }
        public decimal? PtasAnnualmoorageincome { get; set; }
        public decimal? PtasAnnualmoorageincomeBase { get; set; }
        public decimal? PtasAnnualparkingincome { get; set; }
        public decimal? PtasAnnualparkingincomeBase { get; set; }
        public decimal? PtasApartmentexpense { get; set; }
        public decimal? PtasApartmentexpenseBase { get; set; }
        public decimal? PtasApartmentgim { get; set; }
        public decimal? PtasApartmentrentincomemonthly { get; set; }
        public decimal? PtasApartmentrentincomemonthlyBase { get; set; }
        public string PtasAppeals { get; set; }
        public int? PtasAptcondition { get; set; }
        public DateTimeOffset? PtasAssessmentdate { get; set; }
        public int? PtasAssessmentyear { get; set; }
        public bool? PtasAtmarket { get; set; }
        public int? PtasAverageunitsize { get; set; }
        public decimal? PtasBaselandvalue { get; set; }
        public decimal? PtasBaselandvalueBase { get; set; }
        public decimal? PtasBlendedcaprate { get; set; }
        public int? PtasBuildingnetsqft { get; set; }
        public int? PtasBuildingquality { get; set; }
        public decimal? PtasCommercialcaprate { get; set; }
        public decimal? PtasCommercialegi { get; set; }
        public decimal? PtasCommercialegiBase { get; set; }
        public decimal? PtasCommercialgim { get; set; }
        public decimal? PtasCommercialincomevalue { get; set; }
        public decimal? PtasCommercialincomevalueBase { get; set; }
        public int? PtasCommercialnetsqft { get; set; }
        public decimal? PtasCommercialnoi { get; set; }
        public decimal? PtasCommercialnoiBase { get; set; }
        public decimal? PtasCommercialoexfactor { get; set; }
        public decimal? PtasCommercialpgi { get; set; }
        public decimal? PtasCommercialpgiBase { get; set; }
        public decimal? PtasCommercialrentincomeannual { get; set; }
        public decimal? PtasCommercialrentincomeannualBase { get; set; }
        public decimal? PtasCommercialrentrate { get; set; }
        public decimal? PtasCommercialrentrateBase { get; set; }
        public decimal? PtasCommercialvcl { get; set; }
        public bool? PtasCommonlaundry { get; set; }
        public decimal? PtasComparablesalesvalue { get; set; }
        public decimal? PtasComparablesalesvalueBase { get; set; }
        public decimal? PtasComparablesalesvalueminusvacantland { get; set; }
        public decimal? PtasComparablesalesvalueminusvacantlandBase { get; set; }
        public decimal? PtasComparablesalesvalueperunit { get; set; }
        public decimal? PtasComparablesalesvalueperunitBase { get; set; }
        public decimal? PtasComparablesalesweight { get; set; }
        public decimal? PtasCostvaluercnldplusland { get; set; }
        public decimal? PtasCostvaluercnldpluslandBase { get; set; }
        public string PtasDataproblems { get; set; }
        public string PtasEconomicunitparcellist { get; set; }
        public int? PtasEconomicunittype { get; set; }
        public int? PtasEconomicunitvaluationdescription { get; set; }
        public int? PtasEconomicunitvaluationmethod { get; set; }
        public decimal? PtasEffectivegrossincome { get; set; }
        public decimal? PtasEffectivegrossincomeBase { get; set; }
        public int? PtasEffectiveyear { get; set; }
        public int? PtasElevators { get; set; }
        public decimal? PtasEmvminusvacantland { get; set; }
        public decimal? PtasEmvminusvacantlandBase { get; set; }
        public decimal? PtasEmvperunit { get; set; }
        public decimal? PtasEmvperunitBase { get; set; }
        public decimal? PtasEmvweight { get; set; }
        public decimal? PtasEstimatedmarketvalueemv { get; set; }
        public decimal? PtasEstimatedmarketvalueemvBase { get; set; }
        public string PtasFlag { get; set; }
        public decimal? PtasGimblended { get; set; }
        public decimal? PtasGimminusvacantland { get; set; }
        public decimal? PtasGimminusvacantlandBase { get; set; }
        public bool? PtasGovernmentowned { get; set; }
        public decimal? PtasGrossincomemultiplervaluegim { get; set; }
        public decimal? PtasGrossincomemultiplervaluegimBase { get; set; }
        public bool? PtasHoldout { get; set; }
        public decimal? PtasIncomevalue { get; set; }
        public decimal? PtasIncomevalueBase { get; set; }
        public decimal? PtasIncomevalueminusvacantland { get; set; }
        public decimal? PtasIncomevalueminusvacantlandBase { get; set; }
        public decimal? PtasIncomevalueperunit { get; set; }
        public decimal? PtasIncomevalueperunitBase { get; set; }
        public decimal? PtasIncomevaluetopreviousvalue { get; set; }
        public decimal? PtasIncomeweight { get; set; }
        public decimal? PtasManualvalue { get; set; }
        public decimal? PtasManualvalueBase { get; set; }
        public int? PtasMooragecovered { get; set; }
        public int? PtasMoorageopen { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasNeighborhoodrank { get; set; }
        public decimal? PtasNetincome { get; set; }
        public decimal? PtasNetincomeBase { get; set; }
        public int? PtasNumberofparcelsinsale { get; set; }
        public int? PtasNumberofunits { get; set; }
        public decimal? PtasOmmercialoex { get; set; }
        public decimal? PtasOmmercialoexBase { get; set; }
        public decimal? PtasOtherincome { get; set; }
        public decimal? PtasOtherincomeBase { get; set; }
        public int? PtasParkingcoveredsecured { get; set; }
        public int? PtasParkingcoveredunsecured { get; set; }
        public int? PtasParkingopensecured { get; set; }
        public int? PtasParkingopenunsecured { get; set; }
        public decimal? PtasPercentcommercial { get; set; }
        public decimal? PtasPercenttax { get; set; }
        public int? PtasPercentwithview { get; set; }
        public string PtasPermits { get; set; }
        public bool? PtasPool { get; set; }
        public decimal? PtasPotentialgrossincome { get; set; }
        public decimal? PtasPotentialgrossincomeBase { get; set; }
        public int? PtasPresentuse { get; set; }
        public int? PtasPreviousselectedmethod { get; set; }
        public decimal? PtasPrevioustotalvalue { get; set; }
        public decimal? PtasPrevioustotalvalueBase { get; set; }
        public string PtasPropertyname { get; set; }
        public int? PtasRecommendedselectmethod { get; set; }
        public decimal? PtasRecommendedvalue { get; set; }
        public decimal? PtasRecommendedvalueBase { get; set; }
        public decimal? PtasRecommendedvaluetopreviousvalue { get; set; }
        public int? PtasRegion { get; set; }
        public decimal? PtasRentRoommarketrent { get; set; }
        public decimal? PtasRentRoommarketrentBase { get; set; }
        public int? PtasRentStudiocount { get; set; }
        public decimal? PtasRents1bedroom1bathMarketrent { get; set; }
        public decimal? PtasRents1bedroom1bathMarketrentBase { get; set; }
        public int? PtasRents1bedroom1bathavgsqft { get; set; }
        public decimal? PtasRents1bedroom1bathcomprent { get; set; }
        public decimal? PtasRents1bedroom1bathcomprentBase { get; set; }
        public int? PtasRents1bedroom1bathcount { get; set; }
        public int? PtasRents2bedroom1bathavgsqft { get; set; }
        public decimal? PtasRents2bedroom1bathcomprent { get; set; }
        public decimal? PtasRents2bedroom1bathcomprentBase { get; set; }
        public int? PtasRents2bedroom1bathcount { get; set; }
        public decimal? PtasRents2bedroom1bathmarketrent { get; set; }
        public decimal? PtasRents2bedroom1bathmarketrentBase { get; set; }
        public int? PtasRents2bedroom2bathavgsqft { get; set; }
        public decimal? PtasRents2bedroom2bathcomprent { get; set; }
        public decimal? PtasRents2bedroom2bathcomprentBase { get; set; }
        public int? PtasRents2bedroom2bathcount { get; set; }
        public decimal? PtasRents2bedroom2bathmarketrent { get; set; }
        public decimal? PtasRents2bedroom2bathmarketrentBase { get; set; }
        public int? PtasRents3bedroom1bathavgsqft { get; set; }
        public decimal? PtasRents3bedroom1bathcomprent { get; set; }
        public decimal? PtasRents3bedroom1bathcomprentBase { get; set; }
        public int? PtasRents3bedroom1bathcount { get; set; }
        public decimal? PtasRents3bedroom1bathmarketrent { get; set; }
        public decimal? PtasRents3bedroom1bathmarketrentBase { get; set; }
        public int? PtasRents3bedroom2bathavgsqft { get; set; }
        public decimal? PtasRents3bedroom2bathcomprent { get; set; }
        public decimal? PtasRents3bedroom2bathcomprentBase { get; set; }
        public int? PtasRents3bedroom2bathcount { get; set; }
        public decimal? PtasRents3bedroom2bathmarketrent { get; set; }
        public decimal? PtasRents3bedroom2bathmarketrentBase { get; set; }
        public int? PtasRents3bedroom3bathavgsqft { get; set; }
        public decimal? PtasRents3bedroom3bathcomprent { get; set; }
        public decimal? PtasRents3bedroom3bathcomprentBase { get; set; }
        public int? PtasRents3bedroom3bathcount { get; set; }
        public decimal? PtasRents3bedroom3bathmarketrent { get; set; }
        public decimal? PtasRents3bedroom3bathmarketrentBase { get; set; }
        public int? PtasRents4bedroomavgsqft { get; set; }
        public decimal? PtasRents4bedroomcomprent { get; set; }
        public decimal? PtasRents4bedroomcomprentBase { get; set; }
        public int? PtasRents4bedroomcount { get; set; }
        public decimal? PtasRents4bedroommarketrent { get; set; }
        public decimal? PtasRents4bedroommarketrentBase { get; set; }
        public int? PtasRents5bedroomplusavgsqft { get; set; }
        public decimal? PtasRents5bedroompluscomprent { get; set; }
        public decimal? PtasRents5bedroompluscomprentBase { get; set; }
        public int? PtasRents5bedroompluscount { get; set; }
        public decimal? PtasRents5bedroomplusmarketrent { get; set; }
        public decimal? PtasRents5bedroomplusmarketrentBase { get; set; }
        public decimal? PtasRentsMarketrent { get; set; }
        public decimal? PtasRentsMarketrentBase { get; set; }
        public decimal? PtasRentsOpen1bedroomMarketrent { get; set; }
        public decimal? PtasRentsOpen1bedroomMarketrentBase { get; set; }
        public int? PtasRentsOpen1bedroomavgsqft { get; set; }
        public decimal? PtasRentsOpen1bedroomcomprent { get; set; }
        public decimal? PtasRentsOpen1bedroomcomprentBase { get; set; }
        public int? PtasRentsOpen1bedroomcount { get; set; }
        public int? PtasRentsRoomavgsqft { get; set; }
        public decimal? PtasRentsRoomcomprent { get; set; }
        public decimal? PtasRentsRoomcomprentBase { get; set; }
        public int? PtasRentsRoomcount { get; set; }
        public int? PtasRentsStudioavgsqft { get; set; }
        public decimal? PtasRentsStudiocomprent { get; set; }
        public decimal? PtasRentsStudiocomprentBase { get; set; }
        public decimal? PtasRentsStudiomarketrent { get; set; }
        public decimal? PtasRentsStudiomarketrentBase { get; set; }
        public DateTimeOffset? PtasSaleddate { get; set; }
        public decimal? PtasSaleprice { get; set; }
        public decimal? PtasSalepriceBase { get; set; }
        public bool? PtasTenantpaidheat { get; set; }
        public decimal? PtasTotalapartmentmarketrent { get; set; }
        public decimal? PtasTotalapartmentmarketrentBase { get; set; }
        public decimal? PtasTotalexpenses { get; set; }
        public decimal? PtasTotalexpensesBase { get; set; }
        public decimal? PtasTotalexpensespercent { get; set; }
        public decimal? PtasTrendedprice { get; set; }
        public decimal? PtasTrendedpriceBase { get; set; }
        public decimal? PtasTrendedpriceperunit { get; set; }
        public decimal? PtasTrendedpriceperunitBase { get; set; }
        public decimal? PtasUnitsizefactor { get; set; }
        public decimal? PtasVacancyandcreditloss { get; set; }
        public int? PtasValueestimatesupdated { get; set; }
        public DateTimeOffset? PtasValueestimateupdatedate { get; set; }
        public decimal? PtasViewrank { get; set; }
        public decimal? PtasWeightedvalue { get; set; }
        public decimal? PtasWeightedvalueBase { get; set; }
        public decimal? PtasWeightedvalueminusvacantland { get; set; }
        public decimal? PtasWeightedvalueminusvacantlandBase { get; set; }
        public decimal? PtasWeightedvalueperunit { get; set; }
        public decimal? PtasWeightedvalueperunitBase { get; set; }
        public decimal? PtasWeightedvaluetopreviousvalue { get; set; }
        public int? PtasYearbuilt { get; set; }
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
        public Guid? PtasAppraiseridValue { get; set; }
        public Guid? PtasAptneighborhoodidValue { get; set; }
        public Guid? PtasAptvaluationprojectidValue { get; set; }
        public Guid? PtasAssessmentyearlookupidValue { get; set; }
        public Guid? PtasEconomicunitidValue { get; set; }
        public Guid? PtasGeoareaidValue { get; set; }
        public Guid? PtasGeoneighborhoodidValue { get; set; }
        public Guid? PtasLevycodeidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasProjectidValue { get; set; }
        public Guid? PtasPropertytypeidValue { get; set; }
        public Guid? PtasResponsibilityapplgroupValue { get; set; }
        public Guid? PtasSpecialtyareaValue { get; set; }
        public Guid? PtasSupergroupValue { get; set; }
        public Guid? PtasUpdatedbyidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual Systemuser PtasAppraiseridValueNavigation { get; set; }
        public virtual PtasAptneighborhood PtasAptneighborhoodidValueNavigation { get; set; }
        public virtual PtasAptvaluationproject PtasAptvaluationprojectidValueNavigation { get; set; }
        public virtual PtasYear PtasAssessmentyearlookupidValueNavigation { get; set; }
        public virtual PtasEconomicunit PtasEconomicunitidValueNavigation { get; set; }
        public virtual PtasGeoarea PtasGeoareaidValueNavigation { get; set; }
        public virtual PtasGeoneighborhood PtasGeoneighborhoodidValueNavigation { get; set; }
        public virtual PtasLevycode PtasLevycodeidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasCondocomplex PtasProjectidValueNavigation { get; set; }
        public virtual PtasPropertytype PtasPropertytypeidValueNavigation { get; set; }
        public virtual PtasResponsibility PtasResponsibilityapplgroupValueNavigation { get; set; }
        public virtual PtasSpecialtyarea PtasSpecialtyareaValueNavigation { get; set; }
        public virtual PtasSupergroup PtasSupergroupValueNavigation { get; set; }
        public virtual Systemuser PtasUpdatedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasAptcommercialincomeexpense> PtasAptcommercialincomeexpense { get; set; }
        public virtual ICollection<PtasAptcomparablerent> PtasAptcomparablerent { get; set; }
        public virtual ICollection<PtasAptcomparablesale> PtasAptcomparablesale { get; set; }
    }
}

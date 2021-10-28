using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCondocomplex
    {
        public PtasCondocomplex()
        {
            InversePtasMajorcondocomplexidValueNavigation = new HashSet<PtasCondocomplex>();
            InversePtasMasterprojectidValueNavigation = new HashSet<PtasCondocomplex>();
            PtasAbstractproject = new HashSet<PtasAbstractproject>();
            PtasAbstractprojectresultparcel = new HashSet<PtasAbstractprojectresultparcel>();
            PtasAccessorydetail = new HashSet<PtasAccessorydetail>();
            PtasAptlistedrent = new HashSet<PtasAptlistedrent>();
            PtasAptvaluation = new HashSet<PtasAptvaluation>();
            PtasBuildingdetailCommercialuse = new HashSet<PtasBuildingdetailCommercialuse>();
            PtasCondounit = new HashSet<PtasCondounit>();
            PtasLowincomehousingprogram = new HashSet<PtasLowincomehousingprogram>();
            PtasProjectdock = new HashSet<PtasProjectdock>();
            PtasSectionusesqft = new HashSet<PtasSectionusesqft>();
            PtasUnitbreakdown = new HashSet<PtasUnitbreakdown>();
        }

        public Guid PtasCondocomplexid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public bool? PtasAddmanualadjustment { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public string PtasAddr1Line1 { get; set; }
        public bool? PtasAddrestaurantincome { get; set; }
        public bool? PtasAddretailincome { get; set; }
        public int? PtasAmenitypackage { get; set; }
        public bool? PtasApartmentconversion { get; set; }
        public int? PtasAveragesize { get; set; }
        public decimal? PtasAvnrasqft { get; set; }
        public decimal? PtasAvnrasqftBase { get; set; }
        public int? PtasBathroomtype { get; set; }
        public decimal? PtasBathtosleepingroomratio { get; set; }
        public bool? PtasBikestorage { get; set; }
        public string PtasBikestoragedescription { get; set; }
        public int? PtasBuildingcondition { get; set; }
        public int? PtasBuildingquality { get; set; }
        public bool? PtasCalculateparkingincome { get; set; }
        public int? PtasChargingstalls { get; set; }
        public bool? PtasCleaningincluded { get; set; }
        public string PtasComplexdescription { get; set; }
        public int? PtasConstructionclass { get; set; }
        public string PtasContactdescription { get; set; }
        public string PtasContactname { get; set; }
        public int? PtasCoveredoutdoorstoragelinearft { get; set; }
        public bool? PtasDeck { get; set; }
        public string PtasDeckdescription { get; set; }
        public int? PtasDnrland { get; set; }
        public int? PtasEffectiveyear { get; set; }
        public bool? PtasElevators { get; set; }
        public string PtasEmail { get; set; }
        public int? PtasEnergycertification { get; set; }
        public string PtasFax { get; set; }
        public bool? PtasFireplace { get; set; }
        public string PtasFullsitusaddress { get; set; }
        public bool? PtasGroundleases { get; set; }
        public bool? PtasGym { get; set; }
        public string PtasGymdescription { get; set; }
        public bool? PtasInternetincluded { get; set; }
        public int? PtasKitchensinunit { get; set; }
        public decimal? PtasKitchentosleepingroomratio { get; set; }
        public int? PtasLandperunit { get; set; }
        public int? PtasLandtype { get; set; }
        public int? PtasLaundry { get; set; }
        public decimal? PtasLbratio { get; set; }
        public int? PtasLeasingclass { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public int? PtasLegacyrpcomplexid { get; set; }
        public bool? PtasLowincomehousing { get; set; }
        public int? PtasManualadjustmentvalue { get; set; }
        public int? PtasMarinaexistinguse { get; set; }
        public int? PtasMarinasubtype { get; set; }
        public int? PtasMaxnumberofcondounits { get; set; }
        public bool? PtasMfte { get; set; }
        public DateTimeOffset? PtasMfteenddate { get; set; }
        public DateTimeOffset? PtasMftestartdate { get; set; }
        public int? PtasMooragecovered { get; set; }
        public int? PtasMoorageopen { get; set; }
        public string PtasName { get; set; }
        public int? PtasNraperroom { get; set; }
        public int? PtasNumberofanchors { get; set; }
        public int? PtasNumberofbathrooms { get; set; }
        public int? PtasNumberofbuildings { get; set; }
        public int? PtasNumberofcommonkitchens { get; set; }
        public int? PtasNumberofcondounitsremaining { get; set; }
        public int? PtasNumberofjunioranchors { get; set; }
        public int? PtasNumberofstorageunits { get; set; }
        public int? PtasNumberofstories { get; set; }
        public int? PtasNumberofunits { get; set; }
        public string PtasOthersecuritydescription { get; set; }
        public string PtasParcelheadername { get; set; }
        public string PtasParcelheadertext { get; set; }
        public string PtasParcelheadertext2 { get; set; }
        public decimal? PtasParkingoperatingexpensepct { get; set; }
        public decimal? PtasParkingratio { get; set; }
        public int? PtasPercentanchors { get; set; }
        public int? PtasPercentcomplete { get; set; }
        public decimal? PtasPercentlandvaluedecimal { get; set; }
        public int? PtasPercentoffinishedbuildout { get; set; }
        public decimal? PtasPercentownershipdecimal { get; set; }
        public int? PtasPercentremediationcost { get; set; }
        public decimal? PtasPercenttotalvaluedecimal { get; set; }
        public int? PtasPercentwithview { get; set; }
        public string PtasPhone1 { get; set; }
        public bool? PtasPool { get; set; }
        public int? PtasProjectappeal { get; set; }
        public int? PtasProjectlocation { get; set; }
        public string PtasProjectnotes { get; set; }
        public int? PtasProjectsubtype { get; set; }
        public int? PtasProjecttype { get; set; }
        public int? PtasProjectunittype { get; set; }
        public int? PtasProximitytolightrail { get; set; }
        public int? PtasProximitytouw { get; set; }
        public bool? PtasRailspuraccess { get; set; }
        public int? PtasRentalmethod { get; set; }
        public int? PtasRetirementprojectsubtype { get; set; }
        public bool? PtasSecuritysystem { get; set; }
        public int? PtasSecuritytype { get; set; }
        public bool? PtasShowrecordchanges { get; set; }
        public bool? PtasShowsectionuses { get; set; }
        public bool? PtasSingletenancy { get; set; }
        public string PtasSitusaddress { get; set; }
        public DateTimeOffset? PtasSnapshotdatetime { get; set; }
        public int? PtasSnapshottype { get; set; }
        public bool? PtasStorage { get; set; }
        public string PtasStoragedescription { get; set; }
        public int? PtasTemperaturecontrol { get; set; }
        public bool? PtasTenantpaidheat { get; set; }
        public decimal? PtasTotalassessedvalue { get; set; }
        public decimal? PtasTotalassessedvalueBase { get; set; }
        public int? PtasTotalgrosssquarefeet { get; set; }
        public int? PtasTotallinearft { get; set; }
        public int? PtasTotalnetsquarefeet { get; set; }
        public int? PtasTotalsize { get; set; }
        public bool? PtasUsevalueaddcaprate { get; set; }
        public bool? PtasUtilitiesincluded { get; set; }
        public decimal? PtasValaddcaprate { get; set; }
        public string PtasValueadddescription { get; set; }
        public string PtasValueadjustmentdescription { get; set; }
        public int? PtasValuedistributionmethod { get; set; }
        public int? PtasValuedofcoveredsecuredstalls { get; set; }
        public int? PtasValuedofcoveredunsecuredstalls { get; set; }
        public int? PtasValuedofdailystalls { get; set; }
        public int? PtasValuedofmonthlystalls { get; set; }
        public int? PtasValuedofopensecured { get; set; }
        public int? PtasValuedofopenunsecured { get; set; }
        public int? PtasValuedoftotalstalls { get; set; }
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
        public Guid? PtasAccessoryidValue { get; set; }
        public Guid? PtasAddr1CityidValue { get; set; }
        public Guid? PtasAddr1CountryidValue { get; set; }
        public Guid? PtasAddr1StateidValue { get; set; }
        public Guid? PtasAddr1ZipcodeValue { get; set; }
        public Guid? PtasAssociatedparcel2idValue { get; set; }
        public Guid? PtasAssociatedparcel3idValue { get; set; }
        public Guid? PtasAssociatedparcelidValue { get; set; }
        public Guid? PtasContaminationprojectValue { get; set; }
        public Guid? PtasEconomicunitidValue { get; set; }
        public Guid? PtasMajorcondocomplexidValue { get; set; }
        public Guid? PtasMasterprojectidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasParkingdistrictidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasAccessorydetail PtasAccessoryidValueNavigation { get; set; }
        public virtual PtasCity PtasAddr1CityidValueNavigation { get; set; }
        public virtual PtasCountry PtasAddr1CountryidValueNavigation { get; set; }
        public virtual PtasStateorprovince PtasAddr1StateidValueNavigation { get; set; }
        public virtual PtasZipcode PtasAddr1ZipcodeValueNavigation { get; set; }
        public virtual PtasParceldetail PtasAssociatedparcel2idValueNavigation { get; set; }
        public virtual PtasParceldetail PtasAssociatedparcel3idValueNavigation { get; set; }
        public virtual PtasParceldetail PtasAssociatedparcelidValueNavigation { get; set; }
        public virtual PtasContaminationproject PtasContaminationprojectValueNavigation { get; set; }
        public virtual PtasEconomicunit PtasEconomicunitidValueNavigation { get; set; }
        public virtual PtasCondocomplex PtasMajorcondocomplexidValueNavigation { get; set; }
        public virtual PtasCondocomplex PtasMasterprojectidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasParkingdistrict PtasParkingdistrictidValueNavigation { get; set; }
        public virtual ICollection<PtasCondocomplex> InversePtasMajorcondocomplexidValueNavigation { get; set; }
        public virtual ICollection<PtasCondocomplex> InversePtasMasterprojectidValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractproject { get; set; }
        public virtual ICollection<PtasAbstractprojectresultparcel> PtasAbstractprojectresultparcel { get; set; }
        public virtual ICollection<PtasAccessorydetail> PtasAccessorydetail { get; set; }
        public virtual ICollection<PtasAptlistedrent> PtasAptlistedrent { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluation { get; set; }
        public virtual ICollection<PtasBuildingdetailCommercialuse> PtasBuildingdetailCommercialuse { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounit { get; set; }
        public virtual ICollection<PtasLowincomehousingprogram> PtasLowincomehousingprogram { get; set; }
        public virtual ICollection<PtasProjectdock> PtasProjectdock { get; set; }
        public virtual ICollection<PtasSectionusesqft> PtasSectionusesqft { get; set; }
        public virtual ICollection<PtasUnitbreakdown> PtasUnitbreakdown { get; set; }
    }
}

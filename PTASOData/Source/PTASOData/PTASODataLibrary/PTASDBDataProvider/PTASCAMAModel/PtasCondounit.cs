using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCondounit
    {
        public PtasCondounit()
        {
            InversePtasMasterunitidValueNavigation = new HashSet<PtasCondounit>();
            PtasBuildingdetailCommercialuse = new HashSet<PtasBuildingdetailCommercialuse>();
            PtasInspectionhistory = new HashSet<PtasInspectionhistory>();
            PtasSketch = new HashSet<PtasSketch>();
            PtasTask = new HashSet<PtasTask>();
            PtasTaxaccount = new HashSet<PtasTaxaccount>();
            PtasUnitbreakdown = new HashSet<PtasUnitbreakdown>();
        }

        public Guid PtasCondounitid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAccessoryflatvalue { get; set; }
        public decimal? PtasAccessoryflatvalueBase { get; set; }
        public int? PtasAccountstatus { get; set; }
        public int? PtasAccounttype { get; set; }
        public string PtasAddr1Compositeaddress { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public int? PtasAddr1Directionprefix { get; set; }
        public int? PtasAddr1Directionsuffix { get; set; }
        public string PtasAddr1Line2 { get; set; }
        public string PtasAddr1Streetnumber { get; set; }
        public string PtasAddr1Streetnumberfraction { get; set; }
        public int? PtasAddressusage { get; set; }
        public int? PtasAttic { get; set; }
        public int? PtasBuildingnbr { get; set; }
        public string PtasBuildingnumber { get; set; }
        public int? PtasCarportsqft { get; set; }
        public bool? PtasCondo { get; set; }
        public int? PtasCondounitcondition { get; set; }
        public int? PtasDecksqft { get; set; }
        public string PtasDescription { get; set; }
        public int? PtasEffectiveyear { get; set; }
        public int? PtasEndporchsqft { get; set; }
        public bool? PtasEndunit { get; set; }
        public int? PtasEnergyrating { get; set; }
        public int? PtasFinishedbasement { get; set; }
        public bool? PtasFireplace { get; set; }
        public int? PtasFirstfloor { get; set; }
        public int? PtasFloatinghomecondition { get; set; }
        public int? PtasFloatinghomefinishedbasementgrade { get; set; }
        public int? PtasFloatinghomegrade { get; set; }
        public int? PtasFloatinghomeownershiptype { get; set; }
        public int? PtasFloatinghometype { get; set; }
        public string PtasFloornumber { get; set; }
        public int? PtasFlotationtype { get; set; }
        public string PtasFullsitusaddress { get; set; }
        public int? PtasHeatingsystem { get; set; }
        public decimal? PtasImprovementsvalue { get; set; }
        public decimal? PtasImprovementsvalueBase { get; set; }
        public int? PtasInspectionreason { get; set; }
        public decimal? PtasLandvalue { get; set; }
        public decimal? PtasLandvalueBase { get; set; }
        public int? PtasLeasingclass { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public int? PtasLegacyunitid { get; set; }
        public int? PtasLength { get; set; }
        public string PtasLicensenumber { get; set; }
        public int? PtasLinearft { get; set; }
        public string PtasMailingaddresstaxaccount { get; set; }
        public string PtasMakeandmodel { get; set; }
        public string PtasMinornumber { get; set; }
        public int? PtasMobilehomeclass { get; set; }
        public int? PtasMobilehomecondition { get; set; }
        public int? PtasMobilehomesize { get; set; }
        public int? PtasMobilehometype { get; set; }
        public int? PtasMooragetype { get; set; }
        public string PtasName { get; set; }
        public bool? PtasNeedtorevisit { get; set; }
        public decimal? PtasNewconstructionvalue { get; set; }
        public decimal? PtasNewconstructionvalueBase { get; set; }
        public string PtasNotetext { get; set; }
        public int? PtasNumberof12baths { get; set; }
        public int? PtasNumberof34baths { get; set; }
        public int? PtasNumberofbasementparkingspaces { get; set; }
        public int? PtasNumberofbasementtandemspaces { get; set; }
        public int? PtasNumberofbedrooms { get; set; }
        public int? PtasNumberofcarportspaces { get; set; }
        public int? PtasNumberoffullbaths { get; set; }
        public int? PtasNumberofgarageparkingspaces { get; set; }
        public int? PtasNumberofgaragetandemspaces { get; set; }
        public int? PtasNumberofhydraulicparkingspaces { get; set; }
        public int? PtasNumberofopenparkingspaces { get; set; }
        public int? PtasNumberofstories { get; set; }
        public int? PtasOpenporchsqft { get; set; }
        public string PtasOtherparking { get; set; }
        public int? PtasOtherrooms { get; set; }
        public string PtasParcelheadername { get; set; }
        public string PtasParcelheadertext { get; set; }
        public string PtasParcelheadertext2 { get; set; }
        public int? PtasPercentcomplete { get; set; }
        public decimal? PtasPercentlandvaluedecimal { get; set; }
        public int? PtasPercentnetcondition { get; set; }
        public decimal? PtasPercentownershipdecimal { get; set; }
        public decimal? PtasPercenttotalvaluedecimal { get; set; }
        public int? PtasRegressionexclusionreason { get; set; }
        public int? PtasRegressionexclusionreasons { get; set; }
        public int? PtasResunitquality { get; set; }
        public int? PtasRoomadditionalsqft { get; set; }
        public string PtasSeattleid { get; set; }
        public int? PtasSecondfloor { get; set; }
        public DateTimeOffset? PtasSelectdate { get; set; }
        public int? PtasSelectmethod { get; set; }
        public int? PtasSelectreason { get; set; }
        public string PtasSerialnumbervin { get; set; }
        public bool? PtasShowinspectionhistory { get; set; }
        public bool? PtasShowrecordchanges { get; set; }
        public string PtasSitusaddress { get; set; }
        public int? PtasSize { get; set; }
        public int? PtasSkirtlinearft { get; set; }
        public int? PtasSkirttype { get; set; }
        public int? PtasSliplocation { get; set; }
        public decimal? PtasSmallhomeadjustment { get; set; }
        public DateTimeOffset? PtasSnapshotdatetime { get; set; }
        public int? PtasSnapshottype { get; set; }
        public bool? PtasSprinklers { get; set; }
        public int? PtasStoryheight { get; set; }
        public string PtasTaxaccountowner { get; set; }
        public int? PtasTipoutarea { get; set; }
        public bool? PtasTopfloor { get; set; }
        public int? PtasTotalbasement { get; set; }
        public int? PtasTotalliving { get; set; }
        public decimal? PtasTotalvalue { get; set; }
        public decimal? PtasTotalvalueBase { get; set; }
        public bool? PtasUnitinspected { get; set; }
        public DateTimeOffset? PtasUnitinspecteddate { get; set; }
        public int? PtasUnitlocation { get; set; }
        public string PtasUnitnumbertext { get; set; }
        public int? PtasUnitofmeasure { get; set; }
        public int? PtasUnitqualityos { get; set; }
        public int? PtasUnittype { get; set; }
        public int? PtasViewcityorterritorial { get; set; }
        public int? PtasViewlakeorriver { get; set; }
        public int? PtasViewlakewashingtonorlakesammamish { get; set; }
        public int? PtasViewmountain { get; set; }
        public int? PtasViewpugetsound { get; set; }
        public int? PtasWidth { get; set; }
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
        public Guid? PtasAddr1CityidValue { get; set; }
        public Guid? PtasAddr1CountryidValue { get; set; }
        public Guid? PtasAddr1StateidValue { get; set; }
        public Guid? PtasAddr1StreetnameidValue { get; set; }
        public Guid? PtasAddr1StreettypeidValue { get; set; }
        public Guid? PtasAddr1ZipcodeidValue { get; set; }
        public Guid? PtasBuildingidValue { get; set; }
        public Guid? PtasComplexidValue { get; set; }
        public Guid? PtasDockValue { get; set; }
        public Guid? PtasMasterunitidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasPropertytypeidValue { get; set; }
        public Guid? PtasResponsibilityidValue { get; set; }
        public Guid? PtasSelectbyidValue { get; set; }
        public Guid? PtasSketchidValue { get; set; }
        public Guid? PtasSpecialtyareaidValue { get; set; }
        public Guid? PtasSpecialtynbhdidValue { get; set; }
        public Guid? PtasStartassessmentyearidValue { get; set; }
        public Guid? PtasTaxaccountidValue { get; set; }
        public Guid? PtasUnitinspectedbyidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasCity PtasAddr1CityidValueNavigation { get; set; }
        public virtual PtasCountry PtasAddr1CountryidValueNavigation { get; set; }
        public virtual PtasStateorprovince PtasAddr1StateidValueNavigation { get; set; }
        public virtual PtasStreetname PtasAddr1StreetnameidValueNavigation { get; set; }
        public virtual PtasStreettype PtasAddr1StreettypeidValueNavigation { get; set; }
        public virtual PtasZipcode PtasAddr1ZipcodeidValueNavigation { get; set; }
        public virtual PtasBuildingdetail PtasBuildingidValueNavigation { get; set; }
        public virtual PtasCondocomplex PtasComplexidValueNavigation { get; set; }
        public virtual PtasProjectdock PtasDockValueNavigation { get; set; }
        public virtual PtasCondounit PtasMasterunitidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasPropertytype PtasPropertytypeidValueNavigation { get; set; }
        public virtual PtasResponsibility PtasResponsibilityidValueNavigation { get; set; }
        public virtual Systemuser PtasSelectbyidValueNavigation { get; set; }
        public virtual PtasSketch PtasSketchidValueNavigation { get; set; }
        public virtual PtasSpecialtyarea PtasSpecialtyareaidValueNavigation { get; set; }
        public virtual PtasSpecialtyneighborhood PtasSpecialtynbhdidValueNavigation { get; set; }
        public virtual PtasYear PtasStartassessmentyearidValueNavigation { get; set; }
        public virtual PtasTaxaccount PtasTaxaccountidValueNavigation { get; set; }
        public virtual Systemuser PtasUnitinspectedbyidValueNavigation { get; set; }
        public virtual ICollection<PtasCondounit> InversePtasMasterunitidValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetailCommercialuse> PtasBuildingdetailCommercialuse { get; set; }
        public virtual ICollection<PtasInspectionhistory> PtasInspectionhistory { get; set; }
        public virtual ICollection<PtasSketch> PtasSketch { get; set; }
        public virtual ICollection<PtasTask> PtasTask { get; set; }
        public virtual ICollection<PtasTaxaccount> PtasTaxaccount { get; set; }
        public virtual ICollection<PtasUnitbreakdown> PtasUnitbreakdown { get; set; }
    }
}

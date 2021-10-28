using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasBuildingdetail
    {
        public PtasBuildingdetail()
        {
            InversePtasMasterbuildingidValueNavigation = new HashSet<PtasBuildingdetail>();
            PtasAccessorydetail = new HashSet<PtasAccessorydetail>();
            PtasBuildingdetailCommercialuse = new HashSet<PtasBuildingdetailCommercialuse>();
            PtasBuildingsectionfeature = new HashSet<PtasBuildingsectionfeature>();
            PtasCondounit = new HashSet<PtasCondounit>();
            PtasHomeimprovement = new HashSet<PtasHomeimprovement>();
            PtasSalesaggregate = new HashSet<PtasSalesaggregate>();
            PtasSalesbuilding = new HashSet<PtasSalesbuilding>();
            PtasSketch = new HashSet<PtasSketch>();
            PtasUnitbreakdown = new HashSet<PtasUnitbreakdown>();
        }

        public Guid PtasBuildingdetailid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? Ptas12baths { get; set; }
        public int? Ptas1stflrSqft { get; set; }
        public int? Ptas2ndflrSqft { get; set; }
        public int? Ptas34baths { get; set; }
        public decimal? PtasAdditionalcost { get; set; }
        public decimal? PtasAdditionalcostBase { get; set; }
        public int? PtasAdditionallivingquarters { get; set; }
        public int? PtasAddlFireplace { get; set; }
        public string PtasAddr1Compositeaddress { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public int? PtasAddr1Directionprefix { get; set; }
        public int? PtasAddr1Directionsuffix { get; set; }
        public string PtasAddr1Line2 { get; set; }
        public string PtasAddr1Streetnumber { get; set; }
        public string PtasAddr1Streetnumberfraction { get; set; }
        public int? PtasAlternatekey { get; set; }
        public int? PtasAttachedgarageSqft { get; set; }
        public int? PtasBasementgarageSqft { get; set; }
        public int? PtasBedroomnbr { get; set; }
        public string PtasBuildingdescription { get; set; }
        public int? PtasBuildinggrade { get; set; }
        public int? PtasBuildinggrossSqft { get; set; }
        public int? PtasBuildingnbr { get; set; }
        public int? PtasBuildingnetSqft { get; set; }
        public string PtasBuildingnumber { get; set; }
        public int? PtasBuildingobsolescence { get; set; }
        public int? PtasBuildingquality { get; set; }
        public bool? PtasBuiltgreen { get; set; }
        public int? PtasClearheightft { get; set; }
        public int? PtasConstructionclass { get; set; }
        public bool? PtasCranecapacity { get; set; }
        public bool? PtasCraneloading { get; set; }
        public bool? PtasDaylightbasement { get; set; }
        public int? PtasDeckSqft { get; set; }
        public bool? PtasDockloading { get; set; }
        public int? PtasEffectiveyear { get; set; }
        public bool? PtasElevators { get; set; }
        public int? PtasEnclosedporchSqft { get; set; }
        public int? PtasEnergyrating { get; set; }
        public int? PtasFinbsmtSqft { get; set; }
        public int? PtasFrStdFireplace { get; set; }
        public int? PtasFullbathnbr { get; set; }
        public string PtasFullsitusaddress { get; set; }
        public int? PtasGradevariance { get; set; }
        public int? PtasHalfflrSqft { get; set; }
        public int? PtasHeatingsystem { get; set; }
        public string PtasHistguid { get; set; }
        public int? PtasHistyear { get; set; }
        public bool? PtasInvertedfloorplan { get; set; }
        public bool? PtasLeaseholdbuildingm1 { get; set; }
        public int? PtasLeasingclass { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public string PtasMigrationnote { get; set; }
        public int? PtasMultiFireplace { get; set; }
        public string PtasName { get; set; }
        public int? PtasNumberofaggregate { get; set; }
        public int? PtasNumberofcommonwalls { get; set; }
        public int? PtasNumberofelevators { get; set; }
        public int? PtasNumberofstories { get; set; }
        public decimal? PtasNumberofstoriesdecimal { get; set; }
        public int? PtasOnsiteparking { get; set; }
        public int? PtasOpenporchSqft { get; set; }
        public string PtasParcelheadername { get; set; }
        public string PtasParcelheadertext { get; set; }
        public string PtasParcelheadertext2 { get; set; }
        public int? PtasPercentbrickorstone { get; set; }
        public int? PtasPercentcomplete { get; set; }
        public int? PtasPercentnetcondition { get; set; }
        public int? PtasResBasementgrade { get; set; }
        public int? PtasResBuildingcondition { get; set; }
        public int? PtasResHeatsource { get; set; }
        public int? PtasResidentialheatingsystem { get; set; }
        public bool? PtasRestored { get; set; }
        public bool? PtasRooftopdeck { get; set; }
        public int? PtasShape { get; set; }
        public bool? PtasShowrecordchanges { get; set; }
        public int? PtasSingleFireplace { get; set; }
        public string PtasSitusaddress { get; set; }
        public int? PtasSlabthickness { get; set; }
        public DateTimeOffset? PtasSnapshotdatetime { get; set; }
        public int? PtasSnapshottype { get; set; }
        public bool? PtasSolarpanels { get; set; }
        public bool? PtasSprinklers { get; set; }
        public int? PtasStyle { get; set; }
        public int? PtasTotalbsmtSqft { get; set; }
        public int? PtasTotallivingSqft { get; set; }
        public int? PtasUnfinishedFullSqft { get; set; }
        public int? PtasUnfinishedHalfSqft { get; set; }
        public string PtasUnitdescription { get; set; }
        public int? PtasUnits { get; set; }
        public int? PtasUpperflrSqft { get; set; }
        public int? PtasViewutilizationrating { get; set; }
        public int? PtasYearbuilt { get; set; }
        public int? PtasYearrenovated { get; set; }
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
        public Guid? PtasBuildingsectionuseidValue { get; set; }
        public Guid? PtasEffectiveyearidValue { get; set; }
        public Guid? PtasMasterbuildingidValue { get; set; }
        public Guid? PtasParceldetailidValue { get; set; }
        public Guid? PtasPropertytypeidValue { get; set; }
        public Guid? PtasSketchidValue { get; set; }
        public Guid? PtasTaxaccountValue { get; set; }
        public Guid? PtasYearbuiltidValue { get; set; }
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
        public virtual PtasBuildingsectionuse PtasBuildingsectionuseidValueNavigation { get; set; }
        public virtual PtasYear PtasEffectiveyearidValueNavigation { get; set; }
        public virtual PtasBuildingdetail PtasMasterbuildingidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParceldetailidValueNavigation { get; set; }
        public virtual PtasPropertytype PtasPropertytypeidValueNavigation { get; set; }
        public virtual PtasSketch PtasSketchidValueNavigation { get; set; }
        public virtual PtasTaxaccount PtasTaxaccountValueNavigation { get; set; }
        public virtual PtasYear PtasYearbuiltidValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetail> InversePtasMasterbuildingidValueNavigation { get; set; }
        public virtual ICollection<PtasAccessorydetail> PtasAccessorydetail { get; set; }
        public virtual ICollection<PtasBuildingdetailCommercialuse> PtasBuildingdetailCommercialuse { get; set; }
        public virtual ICollection<PtasBuildingsectionfeature> PtasBuildingsectionfeature { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounit { get; set; }
        public virtual ICollection<PtasHomeimprovement> PtasHomeimprovement { get; set; }
        public virtual ICollection<PtasSalesaggregate> PtasSalesaggregate { get; set; }
        public virtual ICollection<PtasSalesbuilding> PtasSalesbuilding { get; set; }
        public virtual ICollection<PtasSketch> PtasSketch { get; set; }
        public virtual ICollection<PtasUnitbreakdown> PtasUnitbreakdown { get; set; }
    }
}

namespace PTASMapTileServicesLibrary.TileFeatureDataProvider.SqlServer.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents feature data for a parcel.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ParcelFeatureData
    {
        /// <summary>
        /// Gets or sets the numeric PIN field.
        /// </summary>
        public double? PinNumeric { get; set; }

        /// <summary>
        /// Gets or sets the PIN field.
        /// </summary>
        [Key]
        public string Pin { get; set; }

        /// <summary>
        /// Gets or sets the RealPropId field.
        /// </summary>
        public int? RealPropId { get; set; }

        /// <summary>
        /// Gets or sets the LandId field.
        /// </summary>
        public int? LandId { get; set; }

        /// <summary>
        /// Gets or sets the major.
        /// </summary>
        public string Major { get; set; }

        /// <summary>
        /// Gets or sets the minor.
        /// </summary>
        public string Minor { get; set; }

        /// <summary>
        /// Gets or sets the land value sq ft.
        /// </summary>
        public decimal? LandValSqFt { get; set; }

        /// <summary>
        /// Gets or sets the property type numeric.
        /// </summary>
        public short? PropTypeNumeric { get; set; }

        /// <summary>
        /// Gets or sets the general classification.
        /// </summary>
        public string GeneralClassif { get; set; }

        /// <summary>
        /// Gets or sets the name of the taxpayer.
        /// </summary>
        public string TaxpayerName { get; set; }

        /// <summary>
        /// Gets or sets the tax status.
        /// </summary>
        public string TaxStatus { get; set; }

        /// <summary>
        /// Gets or sets the BLDG grade.
        /// </summary>
        public byte? BldgGrade { get; set; }

        /// <summary>
        /// Gets or sets the cond description.
        /// </summary>
        public string CondDescr { get; set; }

        /// <summary>
        /// Gets or sets the YrBltRen field.
        /// </summary>
        public string YrBltRen { get; set; }

        /// <summary>
        /// Gets or sets the sq ft tot living.
        /// </summary>
        public int? SqFtTotLiving { get; set; }

        /// <summary>
        /// Gets or sets the current zoning.
        /// </summary>
        public string CurrentZoning { get; set; }

        /// <summary>
        /// Gets or sets the sq ft lot.
        /// </summary>
        public int? SqFtLot { get; set; }

        /// <summary>
        /// Gets or sets the WFNT label.
        /// </summary>
        public string WfntLabel { get; set; }

        /// <summary>
        /// Gets or sets the land prob description part1.
        /// </summary>
        public string LandProbDescrPart1 { get; set; }

        /// <summary>
        /// Gets or sets the land prob description part2.
        /// </summary>
        public string LandProbDescrPart2 { get; set; }

        /// <summary>
        /// Gets or sets the view description.
        /// </summary>
        public string ViewDescr { get; set; }

        /// <summary>
        /// Gets or sets the base land value tax yr.
        /// </summary>
        public int? BaseLandValTaxYr { get; set; }

        /// <summary>
        /// Gets or sets the base land value.
        /// </summary>
        public int? BaseLandVal { get; set; }

        /// <summary>
        /// Gets or sets the BLV sq ft calculate.
        /// </summary>
        public decimal? BLVSqFtCalc { get; set; }

        /// <summary>
        /// Gets or sets the land value.
        /// </summary>
        public int? LandVal { get; set; }

        /// <summary>
        /// Gets or sets the imps value.
        /// </summary>
        public int? ImpsVal { get; set; }

        /// <summary>
        /// Gets or sets the tot value.
        /// </summary>
        public int? TotVal { get; set; }

        /// <summary>
        /// Gets or sets the previous land value.
        /// </summary>
        public int? PrevLandVal { get; set; }

        /// <summary>
        /// Gets or sets the previous imps value.
        /// </summary>
        public int? PrevImpsVal { get; set; }

        /// <summary>
        /// Gets or sets the previous tot value.
        /// </summary>
        public int? PrevTotVal { get; set; }

        /// <summary>
        /// Gets or sets the PCNT CHG land.
        /// </summary>
        public int? PcntChgLand { get; set; }

        /// <summary>
        /// Gets or sets the PCNT CHG imps.
        /// </summary>
        public int? PcntChgImps { get; set; }

        /// <summary>
        /// Gets or sets the PCNT CHG total.
        /// </summary>
        public int? PcntChgTotal { get; set; }

        /// <summary>
        /// Gets or sets the address line.
        /// </summary>
        public string AddrLine { get; set; }

        /// <summary>
        /// Gets or sets the type of the property.
        /// </summary>
        /// </value>
        public char? PropType { get; set; }

        /// <summary>
        /// Gets or sets the application group.
        /// </summary>
        public char? ApplGroup { get; set; }

        /// <summary>
        /// Gets or sets the residential area sub.
        /// </summary>
        /// </value>
        public string ResAreaSub { get; set; }

        /// <summary>
        /// Gets or sets the present use.
        /// </summary>
        public string PresentUse { get; set; }

        /// <summary>
        /// Gets or sets the CMLPredominantUse field.
        /// </summary>
        public string CmlPredominantUse { get; set; }

        /// <summary>
        /// Gets or sets the CmlNetSqFtAllBldg field.
        /// </summary>
        public int? CmlNetSqFtAllBldg { get; set; }

        /// <summary>
        /// Gets or sets the GeoAreaNbhd field.
        /// </summary>
        public string GeoAreaNbhd { get; set; }

        /// <summary>
        /// Gets or sets the SpecAreaNbhd.
        /// </summary>
        public string SpecAreaNbhd { get; set; }

        /// <summary>
        /// Gets or sets the NbrResAccys field.
        /// </summary>
        public short? NbrResAccys { get; set; }

        /// <summary>
        /// Gets or sets the NbrCmlAccys.
        /// </summary>
        /// </value>
        public short? NbrCmlAccys { get; set; }

        /// <summary>
        /// Gets or sets the NewBLVSqFtCalc field.
        /// </summary>
        /// </value>
        public decimal? NewBLVSqFtCalc { get; set; }

        /// <summary>
        /// Gets or sets the PrevLandValSqFt field.
        /// </summary>
        /// </value>
        public decimal? PrevLandValSqFt { get; set; }

        /// <summary>
        /// Gets or sets the CondoYrBuilt field.
        /// </summary>
        /// </value>
        public short? CondoYrBuilt { get; set; }

        /// <summary>
        /// Gets or sets the NewBaseLandVal field.
        /// </summary>
        /// </value>
        public int? NewBaseLandVal { get; set; }

        /// <summary>
        /// Gets or sets the PropName field.
        /// </summary>
        /// </value>
        public string PropName { get; set; }

        /// <summary>
        /// Gets or sets the BLVPcntChg field.
        /// </summary>
        /// </value>
        public int? BLVPcntChg { get; set; }

        /// <summary>
        /// Gets or sets the TrafficValDollars field.
        /// </summary>
        /// </value>
        public int? TrafficValDollars { get; set; }

        /// <summary>
        /// Gets or sets the SpecArea field.
        /// </summary>
        /// </value>
        public string SpecArea { get; set; }

        /// <summary>
        /// Gets or sets the TSP_EMV field.
        /// </summary>
        /// </value>
        public decimal? TSP_EMV { get; set; }

        /// <summary>
        /// Gets or sets the DefaultRender field.
        /// </summary>
        public char? DefaultRender { get; set; }

        /// <summary>
        /// Gets or sets the StreetNbr field.
        /// </summary>
        public string StreetNbr { get; set; }

        /// <summary>
        /// Gets or sets the Inspection field.
        /// </summary>
        public string Inspection { get; set; }

        /// <summary>
        /// Gets or sets the AnyLandProb field.
        /// </summary>
        public string AnyLandProb { get; set; }

        /// <summary>
        /// Gets or sets the PermitTypeToRenderIncompl field.
        /// </summary>
        public string PermitTypeToRenderIncompl { get; set; }

        /// <summary>
        /// Gets or sets the PermitCountIncompl field.
        /// </summary>
        public int? PermitCountIncompl { get; set; }

        /// <summary>
        /// Gets or sets the PermitTypesIncompl field.
        /// </summary>
        public string PermitTypesIncompl { get; set; }

        /// <summary>
        /// Gets or sets the PermitDateRngIncompl field.
        /// </summary>
        public string PermitDateRngIncompl { get; set; }

        /// <summary>
        /// Gets or sets the LastPermitStatusIncompl field.
        /// </summary>
        public string LastPermitStatusIncompl { get; set; }

        /// <summary>
        /// Gets or sets the PermitPcntCompl field.
        /// </summary>
        public string PermitPcntCompl { get; set; }

        /// <summary>
        /// Gets or sets the PermitValsTotIncompl field.
        /// </summary>
        public int? PermitValsTotIncompl { get; set; }

        /// <summary>
        /// Gets or sets the NewConstrVal field.
        /// </summary>
        public int? NewConstrVal { get; set; }

        /// <summary>
        /// Gets or sets the PrevNewConstrVal field.
        /// </summary>
        public int? PrevNewConstrVal { get; set; }

        /// <summary>
        /// Gets or sets the PermitCurIncomplVsCompl field.
        /// </summary>
        public string PermitCurIncomplVsCompl { get; set; }

        /// <summary>
        /// Gets or sets the PP_FieldInspection field.
        /// </summary>
        public string PP_FieldInspection { get; set; }

        /// <summary>
        /// Gets or sets the InspectAssmtYear field.
        /// </summary>
        public string InspectAssmtYear { get; set; }

        /// <summary>
        /// Gets or sets the LastInspectAppr field.
        /// </summary>
        public string LastInspectAppr { get; set; }

        /// <summary>
        /// Gets or sets the PP_ActiveAcct field.
        /// </summary>
        public char? PP_ActiveAcct { get; set; }

        /// <summary>
        /// Gets or sets the SalesVerifSummary field.
        /// </summary>
        public string SalesVerifSummary { get; set; }

        /// <summary>
        /// Gets or sets the SalePrice field.
        /// </summary>
        public int? SalePrice { get; set; }

        /// <summary>
        /// Gets or sets the SalesCountUnverified field.
        /// </summary>
        public int? SalesCountUnverified { get; set; }

        /*/// <summary>
        /// Gets or sets the AttnRequiredDesc field.
        /// </summary>
        public string AttnRequiredDesc { get; set; }*/

        /// <summary>
        /// Gets or sets the ResImpSale field.
        /// </summary>
        public char? ResImpSale { get; set; }

        /// <summary>
        /// Gets or sets the NbrPclsInSale field.
        /// </summary>
        public int? NbrPclsInSale { get; set; }

        /// <summary>
        /// Gets or sets the SalePriceLabels field.
        /// </summary>
        public string SalePriceLabels { get; set; }

        /// <summary>
        /// Gets or sets the VerifAtMkt field.
        /// </summary>
        public char? VerifAtMkt { get; set; }

        /// <summary>
        /// Gets or sets the ResLandSale field.
        /// </summary>
        public char? ResLandSale { get; set; }

        /// <summary>
        /// Gets or sets the SpTotLivImps field.
        /// </summary>
        public decimal? SpTotLivImps { get; set; }

        /// <summary>
        /// Gets or sets the SaleDate field.
        /// </summary>
        public System.DateTime? SaleDate { get; set; }

        /// <summary>
        /// Gets or sets the AVSP field.
        /// </summary>
        public decimal? AVSP { get; set; }

        /// <summary>
        /// Gets or sets the PrevAVSP field.
        /// </summary>
        public decimal? PrevAVSP { get; set; }

        /// <summary>
        /// Gets or sets the SpSqFtLnd field.
        /// </summary>
        public decimal? SpSqFtLnd { get; set; }

        /// <summary>
        /// Gets or sets the SpSqFtLnd field.
        /// </summary>
        public char? ResMHSale { get; set; }

        /// <summary>
        /// Gets or sets the AssignedBoth field.
        /// </summary>
        public string AssignedBoth { get; set; }

        /// <summary>
        /// Gets or sets the TrafficValPct field.
        /// </summary>
        public int? TrafficValPct { get; set; }

        /// <summary>
        /// Gets or sets the GeoArea field.
        /// </summary>
        public string GeoArea { get; set; }

        /// <summary>
        /// Gets or sets the SpecNbhd field.
        /// </summary>
        public string SpecNbhd { get; set; }

        /// <summary>
        /// Gets or sets the EconomicUnit field.
        /// </summary>
        public string EconomicUnit { get; set; }

        /// <summary>
        /// Gets or sets the EconomicUnitName field.
        /// </summary>
        public string EconomicUnitName { get; set; }

        /// <summary>
        /// Gets or sets the AdjacentGolfFairway field.
        /// </summary>
        public char? AdjacentGolfFairway { get; set; }

        /// <summary>
        /// Gets or sets the AdjacentGolfFairway field.
        /// </summary>
        public char? AdjacentGreenbelt { get; set; }

        /// <summary>
        /// Gets or sets the ResAccySale field.
        /// </summary>
        public char? ResAccySale { get; set; }

        /// <summary>
        /// Gets or sets the SaleTypeRP field.
        /// </summary>
        public string SaleTypeRP { get; set; }

        /// <summary>
        /// Gets or sets the VerifiedPriceLabels field.
        /// </summary>
        public string VerifiedPriceLabels { get; set; }

        /// <summary>
        /// Gets or sets the CmlImpSale field.
        /// </summary>
        public char? CmlImpSale { get; set; }

        /// <summary>
        /// Gets or sets the SpNetSqFtImps field.
        /// </summary>
        public decimal? SpNetSqFtImps { get; set; }

        /// <summary>
        /// Gets or sets the FrozPresentUse field.
        /// </summary>
        public string FrozPresentUse { get; set; }

        /// <summary>
        /// Gets or sets the FrozPredominantUse field.
        /// </summary>
        public string FrozPredominantUse { get; set; }

        /// <summary>
        /// Gets or sets the FrozBldgNetSqFt field.
        /// </summary>
        public int? FrozBldgNetSqFt { get; set; }

        /// <summary>
        /// Gets or sets the FrozSqFtLot field.
        /// </summary>
        public int? FrozSqFtLot { get; set; }

        /// <summary>
        /// Gets or sets the CmlLandSale field.
        /// </summary>
        public char? CmlLandSale { get; set; }

        /// <summary>
        /// Gets or sets the CmlAccySale field.
        /// </summary>
        public char? CmlAccySale { get; set; }

        /// <summary>
        /// Gets or sets the PostingStatus field.
        /// </summary>
        public short? PostingStatus { get; set; }

        /// <summary>
        /// Gets or sets the PostingStatusDescr field.
        /// </summary>
        public string PostingStatusDescr { get; set; }

        /// <summary>
        /// Gets or sets the ReviewTypeIncompl field.
        /// </summary>
        public string ReviewTypeIncompl { get; set; }

        /// <summary>
        /// Gets or sets the ReviewTypeIncomplDetails field.
        /// </summary>
        public string ReviewTypeIncomplDetails { get; set; }

        /// <summary>
        /// Gets or sets the ResArea field.
        /// </summary>
        public string ResArea { get; set; }

        /// <summary>
        /// Gets or sets the ResArea field.
        /// </summary>
        public string ResNbhd { get; set; }

        /// <summary>
        /// Gets or sets the Condition field.
        /// </summary>
        public byte? Condition { get; set; }

        /// <summary>
        /// Gets or sets the CondoProjLoc field.
        /// </summary>
        public string CondoProjLoc { get; set; }

        /// <summary>
        /// Gets or sets the CondoProjCond field.
        /// </summary>
        public string CondoProjCond { get; set; }

        /// <summary>
        /// Gets or sets the CondoProjQual field.
        /// </summary>
        public string CondoProjQual { get; set; }

        /// <summary>
        /// Gets or sets the Topography field.
        /// </summary>
        public char? Topography { get; set; }

        /// <summary>
        /// Gets or sets the TopoValPct field.
        /// </summary>
        public int? TopoValPct { get; set; }

        /// <summary>
        /// Gets or sets the TrafficNoise field.
        /// </summary>
        public string TrafficNoise { get; set; }

        /// <summary>
        /// Gets or sets the RoadAccess field.
        /// </summary>
        public string RoadAccess { get; set; }

        /// <summary>
        /// Gets or sets the SewerSystem field.
        /// </summary>
        public string SewerSystem { get; set; }

        /// <summary>
        /// Gets or sets the WaterSystem field.
        /// </summary>
        public string WaterSystem { get; set; }

        /// <summary>
        /// Gets or sets the Cascades field.
        /// </summary>
        public int? Cascades { get; set; }

        /// <summary>
        /// Gets or sets the LakeSammamish field.
        /// </summary>
        public int? LakeSammamish { get; set; }

        /// <summary>
        /// Gets or sets the MtRainier field.
        /// </summary>
        public int? MtRainier { get; set; }

        /// <summary>
        /// Gets or sets the Olympics field.
        /// </summary>
        public int? Olympics { get; set; }

        /// <summary>
        /// Gets or sets the OtherView field.
        /// </summary>
        public int? OtherView { get; set; }

        /// <summary>
        /// Gets or sets the SeattleSkyline field.
        /// </summary>
        public int? SeattleSkyline { get; set; }

        /// <summary>
        /// Gets or sets the SmallLakeRiverCreek field.
        /// </summary>
        public int? SmallLakeRiverCreek { get; set; }

        /// <summary>
        /// Gets or sets the Territorial field.
        /// </summary>
        public int? Territorial { get; set; }

        /// <summary>
        /// Gets or sets the LakeWashington field.
        /// </summary>
        public int? LakeWashington { get; set; }

        /// <summary>
        /// Gets or sets the PugetSound field.
        /// </summary>
        public int? PugetSound { get; set; }

        /// <summary>
        /// Gets or sets the WfntBank field.
        /// </summary>
        public string WfntBank { get; set; }

        /// <summary>
        /// Gets or sets the WfntLocation field.
        /// </summary>
        public string WfntLocation { get; set; }

        /// <summary>
        /// Gets or sets the WfntPoorQuality field.
        /// </summary>
        public char? WfntPoorQuality { get; set; }

        /// <summary>
        /// Gets or sets the WfntProximityInfluence field.
        /// </summary>
        public char? WfntProximityInfluence { get; set; }

        /// <summary>
        /// Gets or sets the WfntRestrictedAccess field.
        /// </summary>
        public string WfntRestrictedAccess { get; set; }

        /// <summary>
        /// Gets or sets the TotalViews field.
        /// </summary>
        public int? TotalViews { get; set; }

        /// <summary>
        /// Gets or sets the CoalValPct field.
        /// </summary>
        public int? CoalValPct { get; set; }

        /// <summary>
        /// Gets or sets the ContamValPct field.
        /// </summary>
        public int? ContamValPct { get; set; }

        /// <summary>
        /// Gets or sets the DrainageValPct field.
        /// </summary>
        public int? DrainageValPct { get; set; }

        /// <summary>
        /// Gets or sets the ErosionValPct field.
        /// </summary>
        public int? ErosionValPct { get; set; }

        /// <summary>
        /// Gets or sets the HundredYrValPct field.
        /// </summary>
        public int? HundredYrValPct { get; set; }

        /// <summary>
        /// Gets or sets the PcntUnusable field.
        /// </summary>
        public byte? PcntUnusable { get; set; }

        /// <summary>
        /// Gets or sets the LandfillValPct field.
        /// </summary>
        public int? LandfillValPct { get; set; }

        /// <summary>
        /// Gets or sets the LandslideValPct field.
        /// </summary>
        public int? LandslideValPct { get; set; }

        /// <summary>
        /// Gets or sets the SeismicValPct field.
        /// </summary>
        public int? SeismicValPct { get; set; }

        /// <summary>
        /// Gets or sets the SensitiveValPct field.
        /// </summary>
        public int? SensitiveValPct { get; set; }

        /// <summary>
        /// Gets or sets the SpeciesValPct field.
        /// </summary>
        public int? SpeciesValPct { get; set; }

        /// <summary>
        /// Gets or sets the SteepSlopeValPct field.
        /// </summary>
        public int? SteepSlopeValPct { get; set; }

        /// <summary>
        /// Gets or sets the StreamValPct field.
        /// </summary>
        public int? StreamValPct { get; set; }

        /// <summary>
        /// Gets or sets the WetlandValPct field.
        /// </summary>
        public int? WetlandValPct { get; set; }

        /// <summary>
        /// Gets or sets the WetlOtherNuisancesandValPct field.
        /// </summary>
        public char? OtherNuisances { get; set; }

        /// <summary>
        /// Gets or sets the OtherProblems field.
        /// </summary>
        public char? OtherProblems { get; set; }

        /// <summary>
        /// Gets or sets the PowerLines field.
        /// </summary>
        public char? PowerLines { get; set; }

        /// <summary>
        /// Gets or sets the PowerLinesValPct field.
        /// </summary>
        public int? PowerLinesValPct { get; set; }

        /// <summary>
        /// Gets or sets the OtherNuisValPct field.
        /// </summary>
        public int? OtherNuisValPct { get; set; }

        /// <summary>
        /// Gets or sets the OtherProblemsValPct field.
        /// </summary>
        public int? OtherProblemsValPct { get; set; }

        /// <summary>
        /// Gets or sets the RoadAccessValPct field.
        /// </summary>
        public int? RoadAccessValPct { get; set; }

        /// <summary>
        /// Gets or sets the WaterProblemsValPct field.
        /// </summary>
        public int? WaterProblemsValPct { get; set; }

        /// <summary>
        /// Gets or sets the DevRightsValPct field.
        /// </summary>
        public int? DevRightsValPct { get; set; }

        /// <summary>
        /// Gets or sets the EasementsValPct field.
        /// </summary>
        public int? EasementsValPct { get; set; }

        /// <summary>
        /// Gets or sets the DeedRestrictValPct field.
        /// </summary>
        public int? DeedRestrictValPct { get; set; }

        /// <summary>
        /// Gets or sets the NativeGrowthValPct field.
        /// </summary>
        public int? NativeGrowthValPct { get; set; }

        /// <summary>
        /// Gets or sets the WaterProblems field.
        /// </summary>
        public char? WaterProblems { get; set; }

        /// <summary>
        /// Gets or sets the OtherDesignation field.
        /// </summary>
        public char? OtherDesignation { get; set; }

        /// <summary>
        /// Gets or sets the DevelopmentRightsPurchased field.
        /// </summary>
        public char? DevelopmentRightsPurchased { get; set; }

        /// <summary>
        /// Gets or sets the DNRLease field.
        /// </summary>
        public char? DNRLease { get; set; }

        /// <summary>
        /// Gets or sets the Easements field.
        /// </summary>
        public char? Easements { get; set; }

        /// <summary>
        /// Gets or sets the DeedRestrictions field.
        /// </summary>
        public char? DeedRestrictions { get; set; }

        /// <summary>
        /// Gets or sets the NativeGrowthProtEsmt field.
        /// </summary>
        public char? NativeGrowthProtEsmt { get; set; }
    }
}

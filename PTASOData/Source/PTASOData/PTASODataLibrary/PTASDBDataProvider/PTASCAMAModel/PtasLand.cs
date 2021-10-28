using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasLand
    {
        public PtasLand()
        {
            InversePtasMasterlandidValueNavigation = new HashSet<PtasLand>();
            PtasEnvironmentalrestriction = new HashSet<PtasEnvironmentalrestriction>();
            PtasLandvaluebreakdown = new HashSet<PtasLandvaluebreakdown>();
            PtasLandvaluecalculation = new HashSet<PtasLandvaluecalculation>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
        }

        public Guid PtasLandid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public DateTimeOffset? PtasAsofvaluationdate { get; set; }
        public int? PtasAssessmentyear { get; set; }
        public bool? PtasAutocalculate { get; set; }
        public decimal? PtasBaselandvalue { get; set; }
        public decimal? PtasBaselandvalueBase { get; set; }
        public bool? PtasCalculateexcessland { get; set; }
        public decimal? PtasCommerciallandvalue { get; set; }
        public decimal? PtasCommerciallandvalueBase { get; set; }
        public decimal? PtasDollarspersquarefoot { get; set; }
        public decimal? PtasDollarspersquarefootBase { get; set; }
        public int? PtasDrysqft { get; set; }
        public int? PtasDrysqftactual { get; set; }
        public decimal? PtasEconomicunitlandvalue { get; set; }
        public decimal? PtasEconomicunitlandvalueBase { get; set; }
        public int? PtasEffectivesqft { get; set; }
        public bool? PtasExcessland { get; set; }
        public int? PtasExcesslandsqft { get; set; }
        public string PtasFullsitusaddress { get; set; }
        public decimal? PtasGrosslandvalue { get; set; }
        public decimal? PtasGrosslandvalueBase { get; set; }
        public int? PtasHbuifimproved { get; set; }
        public int? PtasHbuifvacant { get; set; }
        public decimal? PtasLandacres { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public int? PtasLegacyrplandid { get; set; }
        public string PtasName { get; set; }
        public string PtasParcelheadername { get; set; }
        public string PtasParcelheadertext { get; set; }
        public string PtasParcelheadertext2 { get; set; }
        public int? PtasParking { get; set; }
        public int? PtasPercentbaselandvalue { get; set; }
        public int? PtasPercentunusable { get; set; }
        public int? PtasPresentuse { get; set; }
        public int? PtasRequiredlbratio { get; set; }
        public int? PtasRoadaccess { get; set; }
        public int? PtasSewersystem { get; set; }
        public bool? PtasShowlandvaluebreakdown { get; set; }
        public bool? PtasShowrecordchanges { get; set; }
        public string PtasSitusaddress { get; set; }
        public DateTimeOffset? PtasSnapshotdatetime { get; set; }
        public int? PtasSnapshottype { get; set; }
        public bool? PtasSplitzoning { get; set; }
        public int? PtasSqfttotal { get; set; }
        public int? PtasStreetsurface { get; set; }
        public int? PtasSubmergedsqft { get; set; }
        public int? PtasSubmergedsqftactual { get; set; }
        public int? PtasTaxyear { get; set; }
        public int? PtasTotalsitesperzoning { get; set; }
        public int? PtasTotalsqftactual { get; set; }
        public bool? PtasUnbuildable { get; set; }
        public DateTimeOffset? PtasValuedate { get; set; }
        public int? PtasWatersystem { get; set; }
        public string PtasZoning { get; set; }
        public DateTimeOffset? PtasZoningchangedate { get; set; }
        public string PtasZoningdescription { get; set; }
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
        public Guid? PtasMasterlandidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasLand PtasMasterlandidValueNavigation { get; set; }
        public virtual ICollection<PtasLand> InversePtasMasterlandidValueNavigation { get; set; }
        public virtual ICollection<PtasEnvironmentalrestriction> PtasEnvironmentalrestriction { get; set; }
        public virtual ICollection<PtasLandvaluebreakdown> PtasLandvaluebreakdown { get; set; }
        public virtual ICollection<PtasLandvaluecalculation> PtasLandvaluecalculation { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetail { get; set; }
    }
}

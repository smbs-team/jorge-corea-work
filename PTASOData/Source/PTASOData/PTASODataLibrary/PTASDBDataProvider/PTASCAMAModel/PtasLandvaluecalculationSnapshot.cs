using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasLandvaluecalculationSnapshot
    {
        [Key]
        public Guid PtasLandvaluecalculationid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public bool? PtasAccessrights { get; set; }
        public string PtasAddr1CompositeaddressOneline { get; set; }
        public decimal? PtasAdjustedvalue { get; set; }
        public decimal? PtasAdjustedvalueBase { get; set; }
        public int? PtasAirportnoise { get; set; }
        public int? PtasCharacteristictype { get; set; }
        public bool? PtasDelineationstudy { get; set; }
        public int? PtasDepthfactor { get; set; }
        public string PtasDescription { get; set; }
        public int? PtasDesignationtype { get; set; }
        public decimal? PtasDollaradjustment { get; set; }
        public decimal? PtasDollaradjustmentBase { get; set; }
        public decimal? PtasDollarperlinearft { get; set; }
        public decimal? PtasDollarperlinearftBase { get; set; }
        public decimal? PtasDollarpersqft { get; set; }
        public decimal? PtasDollarpersqftBase { get; set; }
        public decimal? PtasDollarperunit { get; set; }
        public decimal? PtasDollarperunitBase { get; set; }
        public int? PtasEnvironmentalrestriction { get; set; }
        public int? PtasEnvressource { get; set; }
        public decimal? PtasGrosslandvalue { get; set; }
        public decimal? PtasGrosslandvalueBase { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymigrationcode { get; set; }
        public string PtasLegacymigrationinfo { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public int? PtasLinearfootage { get; set; }
        public string PtasName { get; set; }
        public int? PtasNoiselevel { get; set; }
        public int? PtasNuisancetype { get; set; }
        public int? PtasNumberofunits { get; set; }
        public string PtasParcelheadername { get; set; }
        public string PtasParcelheadertext { get; set; }
        public string PtasParcelheadertext2 { get; set; }
        public int? PtasPercentadjustment { get; set; }
        public int? PtasPercentaffected { get; set; }
        public bool? PtasPoorquality { get; set; }
        public bool? PtasProximityinfluence { get; set; }
        public int? PtasQuality { get; set; }
        public int? PtasQuantitytype { get; set; }
        public bool? PtasReallocate { get; set; }
        public int? PtasRestrictedaccess { get; set; }
        public decimal? PtasSitevalue { get; set; }
        public decimal? PtasSitevalueBase { get; set; }
        public DateTimeOffset? PtasSnapshotdateandtime { get; set; }
        public int? PtasSnapshottype { get; set; }
        public int? PtasSquarefootage { get; set; }
        public int? PtasTidelandorshoreland { get; set; }
        public int? PtasTransfertype { get; set; }
        public int? PtasValuemethodcalculation { get; set; }
        public int? PtasValuetype { get; set; }
        public int? PtasViewtype { get; set; }
        public int? PtasWaterfrontbank { get; set; }
        public int? PtasWaterfrontlocation { get; set; }
        public int? PtasWaterfronttype { get; set; }
        public DateTimeOffset? PtasZoningchangedate { get; set; }
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
        public Guid? PtasDesignationtypeidValue { get; set; }
        public Guid? PtasEnvrestypeidValue { get; set; }
        public Guid? PtasLandidValue { get; set; }
        public Guid? PtasMasterlandcharacteristicidValue { get; set; }
        public Guid? PtasNuisancetypeidValue { get; set; }
        public Guid? PtasViewtypeidValue { get; set; }
        public Guid? PtasZoningtypeidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }
    }
}

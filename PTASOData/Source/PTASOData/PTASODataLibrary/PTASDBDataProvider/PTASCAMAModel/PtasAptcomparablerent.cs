using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAptcomparablerent
    {
        public Guid PtasAptcomparablerentid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAbsoluteadjustsment { get; set; }
        public decimal? PtasAdjustedrent { get; set; }
        public decimal? PtasAdjustedrentBase { get; set; }
        public decimal? PtasAgeadjustment { get; set; }
        public decimal? PtasAirportnoiseadjustment { get; set; }
        public int? PtasAssessmentyear { get; set; }
        public int? PtasComp { get; set; }
        public decimal? PtasConditionadjustment { get; set; }
        public decimal? PtasDistancemetriccombinedadjustment { get; set; }
        public decimal? PtasLocationadjustment { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasNetadjustment { get; set; }
        public decimal? PtasNumberofbathroomsadjustment { get; set; }
        public decimal? PtasNumberofbedroomsadjustment { get; set; }
        public decimal? PtasPooladjustment { get; set; }
        public decimal? PtasProximityadjustment { get; set; }
        public int? PtasProximitycode { get; set; }
        public decimal? PtasQualityadjustment { get; set; }
        public decimal? PtasUnitsizeadjustment1 { get; set; }
        public decimal? PtasUnitsizeadjustment2 { get; set; }
        public decimal? PtasUnittypeadjustment { get; set; }
        public decimal? PtasViewadjustment1 { get; set; }
        public decimal? PtasViewadjustment2 { get; set; }
        public decimal? PtasWeightingdenominator { get; set; }
        public decimal? PtasYearbuiltadjustment { get; set; }
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
        public Guid? PtasRentidValue { get; set; }
        public Guid? PtasRentsubjectidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasAptlistedrent PtasRentidValueNavigation { get; set; }
        public virtual PtasAptvaluation PtasRentsubjectidValueNavigation { get; set; }
    }
}

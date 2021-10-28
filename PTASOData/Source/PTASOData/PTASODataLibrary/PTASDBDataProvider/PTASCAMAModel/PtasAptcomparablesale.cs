using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAptcomparablesale
    {
        public Guid PtasAptcomparablesaleid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAbsoluteadjustmentwithoutlocation { get; set; }
        public decimal? PtasAbsoluteadjustsment { get; set; }
        public decimal? PtasAdjustedsaleprice { get; set; }
        public decimal? PtasAdjustedsalepriceBase { get; set; }
        public decimal? PtasAdjustedsalepriceperunit { get; set; }
        public decimal? PtasAdjustedsalepriceperunitBase { get; set; }
        public decimal? PtasAgeadjustment { get; set; }
        public decimal? PtasAggregateabsoluteadjustment { get; set; }
        public decimal? PtasAirportnoiseadjustment { get; set; }
        public int? PtasAssessmentyear { get; set; }
        public decimal? PtasAverageunitsizeadjustment { get; set; }
        public decimal? PtasBuildingqualityadjustment { get; set; }
        public int? PtasComp { get; set; }
        public decimal? PtasComplementofabsoluteadjustment { get; set; }
        public decimal? PtasCompweight { get; set; }
        public decimal? PtasConditionadjustment { get; set; }
        public decimal? PtasDistancemetriccombinedadjustment { get; set; }
        public decimal? PtasLocationadjustment { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasNetadjustment { get; set; }
        public decimal? PtasNetadjustmentwithoutlocation { get; set; }
        public decimal? PtasNumberofunitsadjustment { get; set; }
        public decimal? PtasPercentcommercialadjustment { get; set; }
        public decimal? PtasPooladjustment { get; set; }
        public int? PtasProximitycode { get; set; }
        public decimal? PtasProximitycodeadjustment { get; set; }
        public decimal? PtasReconciledcomparablevalueperunit { get; set; }
        public decimal? PtasReconciledcomparablevalueperunitBase { get; set; }
        public decimal? PtasSaledateadjustment { get; set; }
        public decimal? PtasTrendedsalepriceperunit { get; set; }
        public decimal? PtasTrendedsalepriceperunitBase { get; set; }
        public decimal? PtasViewadjustment { get; set; }
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
        public Guid? PtasComparablesalesvaluationsubjectidValue { get; set; }
        public Guid? PtasSaleidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasAptvaluation PtasComparablesalesvaluationsubjectidValueNavigation { get; set; }
        public virtual PtasAptavailablecomparablesale PtasSaleidValueNavigation { get; set; }
    }
}

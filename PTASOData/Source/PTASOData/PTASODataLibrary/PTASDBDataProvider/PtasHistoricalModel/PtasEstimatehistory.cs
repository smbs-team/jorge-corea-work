using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasHistoricalModel
{
    public partial class PtasEstimateHistory
    {
        public Guid? AssessmentYearGuid { get; set; }
        public string AssessmentYearIdName { get; set; }
        public Guid? BuildingGuid { get; set; }
        public string BuildingIdName { get; set; }
        public DateTime? CalculationDate { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedOn { get; set; }
        [Key]
        public Guid EstimateHistoryGuid { get; set; }
        public string EstimateType { get; set; }
        public int? EstimateTypeId { get; set; }
        public double? ImpsValue { get; set; }
        public Guid? LandId { get; set; }
        public string LandIdName { get; set; }
        public double? LandValue { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ParcelGuid { get; set; }
        public string ParcelIdName { get; set; }
        public string RecName { get; set; }
        public Guid? RollYearGuid { get; set; }
        public string RollYearName { get; set; }
        public Guid? TaxAccountId { get; set; }
        public string TaxAccountIdName { get; set; }
        public double? TotalValue { get; set; }
        public Guid? PpTaxAccountId { get; set; }
        public string PpTaxAccountIdName { get; set; }
        public string EstimateSrc { get; set; }
        public int? ExcessLandSqFt { get; set; }
        public double? ExcessLandValue { get; set; }
        public bool? NoExcessLandWarning { get; set; }
        public int? BldgNbr { get; set; }
        public string RecType { get; set; }
        public Guid? UnitGuid { get; set; }
        public string UnitIdName { get; set; }
    }
}

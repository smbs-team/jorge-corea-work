using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasHistoricalModel
{
    public partial class PtasFloatingHomeValuation
    {
        [Key]
        public Guid FloatingHomeValuationId { get; set; }
        public string RecordName { get; set; }
        public Guid? ParcelGuid { get; set; }
        public string ParcelIdName { get; set; }
        public Guid? AssessmentYearGuid { get; set; }
        public string AssessmentYearIdName { get; set; }
        public Guid? FloatingHomeUnitGuid { get; set; }
        public string FloatingHomeUnitIdName { get; set; }
        public decimal? SlipGradeValue { get; set; }
        public decimal? SubjectParcelSlipValue { get; set; }
        public decimal? DnrSlipValue { get; set; }
        public decimal? CitySlipValue { get; set; }
        public decimal? RcnperSqft { get; set; }
        public decimal? RcnldperSqft { get; set; }
        public decimal? LivingValue { get; set; }
        public decimal? BasementValue { get; set; }
        public decimal? TotalHomeValue { get; set; }
        public decimal? SmallHomeAdjustmentValue { get; set; }
        public decimal? PcntNetConditionValue { get; set; }
        public decimal? Rcnld { get; set; }
        public Guid? FloatingHomeProjectGuid { get; set; }
        public string FloatingHomeProjectIdName { get; set; }
    }
}

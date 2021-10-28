using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasHistoricalModel
{
    public partial class PtasFloatingHomeReplacementCostRate
    {
        [Key]
        public Guid FloatingHomeReplacementCostRateId { get; set; }
        public Guid? SpecialityAreaGuid { get; set; }
        public string SpecialityAreaIdName { get; set; }
        public Guid? SpecialityNeighborhoodGuid { get; set; }
        public string SpecialityNeighborhoodIdName { get; set; }
        public Guid? AssessmentYearGuid { get; set; }
        public string AssessmentYearIdName { get; set; }
        public decimal? GradeAverageMinus { get; set; }
        public decimal? GradeAverage { get; set; }
        public decimal? GradeAveragePlus { get; set; }
        public decimal? GradeGoodMinus { get; set; }
        public decimal? GradeGood { get; set; }
        public decimal? GradeGoodPlus { get; set; }
        public decimal? GradeExcellentMinus { get; set; }
        public decimal? GradeExcellent { get; set; }
        public decimal? GradeExcellentPlus { get; set; }
    }
}

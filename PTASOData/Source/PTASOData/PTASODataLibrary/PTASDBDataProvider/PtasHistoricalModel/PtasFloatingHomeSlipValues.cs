using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasHistoricalModel
{
    public partial class PtasFloatingHomeSlipValues
    {
        [Key]
        public Guid FloatingHomeSlipValuesId { get; set; }
        public Guid? SpecialityAreaGuid { get; set; }
        public string SpecialityAreaIdName { get; set; }
        public Guid? AssessmentYearGuid { get; set; }
        public string AssessmentYearIdName { get; set; }
        public int? SlipQuality { get; set; }
        public decimal? Grade1 { get; set; }
        public decimal? Grade2 { get; set; }
        public decimal? Grade3 { get; set; }
        public decimal? Grade4 { get; set; }
        public decimal? Grade5 { get; set; }
        public decimal? Grade6 { get; set; }
        public decimal? Grade7 { get; set; }
    }
}

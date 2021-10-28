using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasHistoricalModel
{
    public partial class PtasIncomemodeldetail
    {
        public string Name { get; set; }
        public Guid? GeoAreaId { get; set; }
        public Guid? GeoNbhdId { get; set; }
        public Guid? AssessmentYearId { get; set; }
        public Guid? SpecAreaId { get; set; }
        public Guid? SpecNbhdId { get; set; }
        public Guid? IncomeModelId { get; set; }
        public int? MinSqFt { get; set; }
        public int? MaxSqFt { get; set; }
        public Guid? MinEffectiveYearId { get; set; }
        public Guid? MaxEffectiveYearId { get; set; }
        public int? Stratification { get; set; }
        public int? OperatingExpenseCalc { get; set; }
        public string CurrentSectionUseCodes { get; set; }
        public decimal? Grade1 { get; set; }
        public decimal? Grade2 { get; set; }
        public decimal? Grade3 { get; set; }
        public decimal? Grade4 { get; set; }
        public decimal? Grade5 { get; set; }
        public decimal? Grade6 { get; set; }
        public decimal? Grade7 { get; set; }
        public string RateType { get; set; }
        public int? OrderRows { get; set; }
        [Key]
        public Guid RowGuid { get; set; }
    }
}

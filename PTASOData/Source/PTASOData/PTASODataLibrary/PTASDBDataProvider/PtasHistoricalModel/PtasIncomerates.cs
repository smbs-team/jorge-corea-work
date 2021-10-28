using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasHistoricalModel
{
    public partial class PtasIncomerates
    {
        public string Name { get; set; }
        public Guid? GeoAreaId { get; set; }
        public Guid? GeoNbhdId { get; set; }
        public Guid? AssesmentYearId { get; set; }
        public Guid? SpecAreaId { get; set; }
        public Guid? SpecNbhdId { get; set; }
        public Guid? IncomeModelId { get; set; }
        public int? MinSqFt { get; set; }
        public int? MaxSqFt { get; set; }
        public Guid? MinEffectiveYearId { get; set; }
        public Guid? MaxEffectiveYearId { get; set; }
        public Guid? CurrentSectionUseCodes { get; set; }
        public int? LeasingClass { get; set; }
        public int? BuildingQuality { get; set; }
        public decimal? Rent { get; set; }
        public decimal? VacancyAndCollectionLoss { get; set; }
        public decimal? OperatingExpensePercent { get; set; }
        public decimal? CapitalizationRate { get; set; }
        [Key]
        public Guid RowGuid { get; set; }
    }
}

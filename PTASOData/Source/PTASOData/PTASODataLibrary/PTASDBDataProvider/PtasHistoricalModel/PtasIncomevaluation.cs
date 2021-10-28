using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasHistoricalModel
{
    public partial class PtasIncomevaluation
    {
        public string Name { get; set; }
        public Guid? ParcelId { get; set; }
        public Guid? AccountNumberId { get; set; }
        public Guid? AssessmentYearId { get; set; }
        public Guid? BuildingId { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? SectionUseId { get; set; }
        [Key]
        public Guid? IncomeModelDetailId { get; set; }
        public Guid? ParkingDistrictId { get; set; }
        public int? NetSqft { get; set; }
        public decimal? DollarPerSqFt { get; set; }
        public string ExceptionCode { get; set; }
        public int? EstimateType { get; set; }
        public Guid? EstimateHistoryId { get; set; }
        public decimal? Rent { get; set; }
        public decimal? VacancyAndLossCollection { get; set; }
        public decimal? OperatingExpensePercent { get; set; }
        public decimal? CapRate { get; set; }
        public decimal? PotentialGrossIncome { get; set; }
        public decimal? EffectiveGrossIncome { get; set; }
        public decimal? NetOperatingIncome { get; set; }
        public decimal? IndicatedValue { get; set; }
        public decimal? WeightedCapAmt { get; set; }
        public decimal? MonthlyRate { get; set; }
        public decimal? DailyRate { get; set; }
        public decimal? ParkingOccupancy { get; set; }
        public decimal? OperatingExpensesParking { get; set; }
        public decimal? CapRateParking { get; set; }
        public int? MonthlySpaces { get; set; }
        public int? DailySpaces { get; set; }
        public decimal? EffectiveGrossIncomeParking { get; set; }
        public decimal? NetOperatingIncomeParking { get; set; }
        public decimal? WeightedCapAmtParking { get; set; }
        public int RowId { get; set; }
    }
}

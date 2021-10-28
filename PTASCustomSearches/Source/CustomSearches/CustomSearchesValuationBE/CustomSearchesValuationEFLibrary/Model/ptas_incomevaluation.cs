using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_incomevaluation
    {
        public string name { get; set; }
        public Guid? parcelId { get; set; }
        public Guid? accountNumberId { get; set; }
        public Guid? assessmentYearId { get; set; }
        public Guid? buildingId { get; set; }
        public Guid? projectId { get; set; }
        public Guid? sectionUseId { get; set; }
        public Guid? incomeModelDetailId { get; set; }
        public Guid? parkingDistrictId { get; set; }
        public int? netSqft { get; set; }
        public decimal? dollarPerSqFt { get; set; }
        public string exceptionCode { get; set; }
        public int? estimateType { get; set; }
        public Guid? estimateHistoryId { get; set; }
        public decimal? rent { get; set; }
        public decimal? vacancyAndLossCollection { get; set; }
        public decimal? operatingExpensePercent { get; set; }
        public decimal? capRate { get; set; }
        public decimal? potentialGrossIncome { get; set; }
        public decimal? effectiveGrossIncome { get; set; }
        public decimal? netOperatingIncome { get; set; }
        public decimal? indicatedValue { get; set; }
        public decimal? weightedCapAmt { get; set; }
        public decimal? monthlyRate { get; set; }
        public decimal? dailyRate { get; set; }
        public decimal? parkingOccupancy { get; set; }
        public decimal? operatingExpensesParking { get; set; }
        public decimal? capRateParking { get; set; }
        public int? monthlySpaces { get; set; }
        public int? dailySpaces { get; set; }
        public decimal? effectiveGrossIncomeParking { get; set; }
        public decimal? netOperatingIncomeParking { get; set; }
        public decimal? weightedCapAmtParking { get; set; }
        public int RowID { get; set; }
    }
}

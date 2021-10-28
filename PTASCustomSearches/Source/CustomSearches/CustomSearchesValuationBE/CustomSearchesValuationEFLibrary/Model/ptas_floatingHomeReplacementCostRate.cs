using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_floatingHomeReplacementCostRate
    {
        public Guid floatingHomeReplacementCostRateId { get; set; }
        public Guid? specialityAreaGuid { get; set; }
        public string specialityAreaIdName { get; set; }
        public Guid? specialityNeighborhoodGuid { get; set; }
        public string specialityNeighborhoodIdName { get; set; }
        public Guid? assessmentYearGuid { get; set; }
        public string assessmentYearIdName { get; set; }
        public decimal? gradeAverageMinus { get; set; }
        public decimal? gradeAverage { get; set; }
        public decimal? gradeAveragePlus { get; set; }
        public decimal? gradeGoodMinus { get; set; }
        public decimal? gradeGood { get; set; }
        public decimal? gradeGoodPlus { get; set; }
        public decimal? gradeExcellentMinus { get; set; }
        public decimal? gradeExcellent { get; set; }
        public decimal? gradeExcellentPlus { get; set; }
    }
}

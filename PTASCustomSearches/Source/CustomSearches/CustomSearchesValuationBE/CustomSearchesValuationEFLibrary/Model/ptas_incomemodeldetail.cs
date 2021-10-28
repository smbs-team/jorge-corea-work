using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_incomemodeldetail
    {
        public string name { get; set; }
        public Guid? geoAreaId { get; set; }
        public Guid? geoNbhdId { get; set; }
        public Guid? assessmentYearId { get; set; }
        public Guid? specAreaId { get; set; }
        public Guid? specNbhdId { get; set; }
        public Guid? incomeModelId { get; set; }
        public int? minSqFt { get; set; }
        public int? maxSqFt { get; set; }
        public Guid? minEffectiveYearId { get; set; }
        public Guid? maxEffectiveYearId { get; set; }
        public int? stratification { get; set; }
        public int? operatingExpenseCalc { get; set; }
        public string currentSectionUseCodes { get; set; }
        public decimal? grade1 { get; set; }
        public decimal? grade2 { get; set; }
        public decimal? grade3 { get; set; }
        public decimal? grade4 { get; set; }
        public decimal? grade5 { get; set; }
        public decimal? grade6 { get; set; }
        public decimal? grade7 { get; set; }
        public string rateType { get; set; }
        public int RowID { get; set; }
    }
}

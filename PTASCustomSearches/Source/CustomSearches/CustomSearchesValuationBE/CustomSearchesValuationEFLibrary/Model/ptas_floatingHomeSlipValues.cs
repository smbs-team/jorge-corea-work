using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_floatingHomeSlipValues
    {
        public Guid floatingHomeSlipValuesId { get; set; }
        public Guid? specialityAreaGuid { get; set; }
        public string specialityAreaIdName { get; set; }
        public Guid? assessmentYearGuid { get; set; }
        public string assessmentYearIdName { get; set; }
        public int? slipQuality { get; set; }
        public decimal? grade1 { get; set; }
        public decimal? grade2 { get; set; }
        public decimal? grade3 { get; set; }
        public decimal? grade4 { get; set; }
        public decimal? grade5 { get; set; }
        public decimal? grade6 { get; set; }
        public decimal? grade7 { get; set; }
    }
}

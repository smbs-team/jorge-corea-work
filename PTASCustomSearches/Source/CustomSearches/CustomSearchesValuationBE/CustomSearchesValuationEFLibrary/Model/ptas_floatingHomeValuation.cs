using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_floatingHomeValuation
    {
        public Guid floatingHomeValuationId { get; set; }
        public string recordName { get; set; }
        public Guid? parcelGuid { get; set; }
        public string parcelIdName { get; set; }
        public Guid? assessmentYearGuid { get; set; }
        public string assessmentYearIdName { get; set; }
        public Guid? floatingHomeUnitGuid { get; set; }
        public string floatingHomeUnitIdName { get; set; }
        public decimal? slipGradeValue { get; set; }
        public decimal? subjectParcelSlipValue { get; set; }
        public decimal? dnrSlipValue { get; set; }
        public decimal? citySlipValue { get; set; }
        public decimal? RCNperSqft { get; set; }
        public decimal? RCNLDperSqft { get; set; }
        public decimal? livingValue { get; set; }
        public decimal? basementValue { get; set; }
        public decimal? totalHomeValue { get; set; }
        public decimal? smallHomeAdjustmentValue { get; set; }
        public decimal? pcntNetConditionValue { get; set; }
        public decimal? RCNLD { get; set; }
        public Guid? floatingHomeProjectGuid { get; set; }
        public string floatingHomeProjectIdName { get; set; }
    }
}

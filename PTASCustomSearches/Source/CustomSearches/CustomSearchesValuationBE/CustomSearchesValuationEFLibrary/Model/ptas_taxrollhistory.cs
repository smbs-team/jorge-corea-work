using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_taxRollHistory
    {
        public string acctStat { get; set; }
        public double? appraiserImpValue { get; set; }
        public double? appraiserLandValue { get; set; }
        public double? appraiserTotalValue { get; set; }
        public bool? isCurrent { get; set; }
        public Guid? levyCodeId { get; set; }
        public string levyCodeIdName { get; set; }
        public Guid? omitYearId { get; set; }
        public string omitYearIdName { get; set; }
        public Guid? parcelGuid { get; set; }
        public string parcelIdName { get; set; }
        public string receivableType { get; set; }
        public string recName { get; set; }
        public double? taxableImpValue { get; set; }
        public double? taxableLandValue { get; set; }
        public double? taxableTotal { get; set; }
        public string taxableValueReason { get; set; }
        public Guid? taxAccountId { get; set; }
        public string taxAccountIdName { get; set; }
        public Guid taxRollHistoryGuid { get; set; }
        public string taxStat { get; set; }
        public Guid? taxYearId { get; set; }
        public string taxYearIdName { get; set; }
        public DateTime? modifiedOn { get; set; }
        public int? newDollars { get; set; }
    }
}

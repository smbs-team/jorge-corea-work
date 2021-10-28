using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_appraisalHistory
    {
        public Guid appraisalHistoryGuid { get; set; }
        public DateTime? appraisedDate { get; set; }
        public Guid? appraiserGuid { get; set; }
        public string appraiserName { get; set; }
        public string ApprMethod { get; set; }
        public int? ApprMethodId { get; set; }
        public string createdByName { get; set; }
        public DateTime? createdOn { get; set; }
        public bool? hasNote { get; set; }
        public double? impsValue { get; set; }
        public int? interfaceFlag { get; set; }
        public int? landId { get; set; }
        public double? landValue { get; set; }
        public string modifiedByName { get; set; }
        public DateTime? modifiedOn { get; set; }
        public double? newConstrValue { get; set; }
        public string note { get; set; }
        public Guid? parcelGuid { get; set; }
        public string parcelIdName { get; set; }
        public DateTime? postDate { get; set; }
        public int? realPropId { get; set; }
        public string recName { get; set; }
        public string revalOrMaint { get; set; }
        public string RollYear { get; set; }
        public int? splitCode { get; set; }
        public Guid? taxAccountGuid { get; set; }
        public string taxAccountIdName { get; set; }
        public Guid? taxYearGuid { get; set; }
        public string taxYearIdName { get; set; }
        public double? totalValue { get; set; }
        public string transactionBy { get; set; }
        public DateTime? transactionDate { get; set; }
        public string valuationReason { get; set; }
        public int? valuationReasonId { get; set; }
        public Guid? LandGuid { get; set; }
        public string LandIdName { get; set; }
        public decimal? PercentChange { get; set; }
        public string InterfaceFlagDesc { get; set; }
        public string ImageCode { get; set; }
    }
}

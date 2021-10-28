using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_estimateHistory
    {
        public Guid? assessmentYearGuid { get; set; }
        public string assessmentYearIdName { get; set; }
        public Guid? buildingGuid { get; set; }
        public string buildingIdName { get; set; }
        public DateTime? calculationDate { get; set; }
        public string createdByName { get; set; }
        public DateTime? createdOn { get; set; }
        public Guid estimateHistoryGuid { get; set; }
        public string estimateType { get; set; }
        public int? estimateTypeId { get; set; }
        public double? impsValue { get; set; }
        public Guid? landId { get; set; }
        public string landIdName { get; set; }
        public double? landValue { get; set; }
        public string modifiedByName { get; set; }
        public DateTime? modifiedOn { get; set; }
        public Guid? parcelGuid { get; set; }
        public string parcelIdName { get; set; }
        public string recName { get; set; }
        public Guid? rollYearGuid { get; set; }
        public string rollYearName { get; set; }
        public Guid? taxAccountId { get; set; }
        public string taxAccountIdName { get; set; }
        public double? totalValue { get; set; }
        public Guid? ppTaxAccountId { get; set; }
        public string ppTaxAccountIdName { get; set; }
        public string estimateSrc { get; set; }
        public int? ExcessLandSqFt { get; set; }
        public double? ExcessLandValue { get; set; }
        public bool? NoExcessLandWarning { get; set; }
        public int? BldgNbr { get; set; }
    }
}

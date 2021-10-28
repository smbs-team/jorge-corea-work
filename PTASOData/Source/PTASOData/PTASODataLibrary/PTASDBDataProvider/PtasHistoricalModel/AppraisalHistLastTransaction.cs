using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasHistoricalModel
{
    public partial class AppraisalHistLastTransaction
    {
        public long? RowNum { get; set; }
        [Key]
        public Guid AppraisalHistoryGuid { get; set; }
        public DateTime? AppraisedDate { get; set; }
        public Guid? AppraiserGuid { get; set; }
        public string AppraiserName { get; set; }
        public string ApprMethod { get; set; }
        public int? ApprMethodId { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? HasNote { get; set; }
        public double? ImpsValue { get; set; }
        public int? InterfaceFlag { get; set; }
        public int? LandId { get; set; }
        public double? LandValue { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public double? NewConstrValue { get; set; }
        public string Note { get; set; }
        public Guid? ParcelGuid { get; set; }
        public string ParcelIdName { get; set; }
        public DateTime? PostDate { get; set; }
        public int? RealPropId { get; set; }
        public string RecName { get; set; }
        public string RevalOrMaint { get; set; }
        public string RollYear { get; set; }
        public int? SplitCode { get; set; }
        public Guid? TaxAccountGuid { get; set; }
        public string TaxAccountIdName { get; set; }
        public Guid? TaxYearGuid { get; set; }
        public string TaxYearIdName { get; set; }
        public double? TotalValue { get; set; }
        public string TransactionBy { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string ValuationReason { get; set; }
        public int? ValuationReasonId { get; set; }
        public Guid? LandGuid { get; set; }
        public string LandIdName { get; set; }
        public decimal? PercentChange { get; set; }
        public string InterfaceFlagDesc { get; set; }
        public string ImageCode { get; set; }
    }
}

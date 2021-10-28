using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasHistoricalModel
{
    public partial class PtasTaxRollHistory
    {
        public string AcctStat { get; set; }
        public double? AppraiserImpValue { get; set; }
        public double? AppraiserLandValue { get; set; }
        public double? AppraiserTotalValue { get; set; }
        public bool? IsCurrent { get; set; }
        public Guid? LevyCodeId { get; set; }
        public string LevyCodeIdName { get; set; }
        public Guid? OmitYearId { get; set; }
        public string OmitYearIdName { get; set; }
        public Guid? ParcelGuid { get; set; }
        public string ParcelIdName { get; set; }
        public string ReceivableType { get; set; }
        public string RecName { get; set; }
        public double? TaxableImpValue { get; set; }
        public double? TaxableLandValue { get; set; }
        public double? TaxableTotal { get; set; }
        public string TaxableValueReason { get; set; }
        public Guid? TaxAccountId { get; set; }
        public string TaxAccountIdName { get; set; }
        [Key]
        public Guid TaxRollHistoryGuid { get; set; }
        public string TaxStat { get; set; }
        public Guid? TaxYearId { get; set; }
        public string TaxYearIdName { get; set; }
        public DateTime Modifiedon { get; set; }
        public int? NewDollars { get; set; }
        public string RecType { get; set; }
        public Guid? UnitGuid { get; set; }
        public string UnitIdName { get; set; }
    }
}

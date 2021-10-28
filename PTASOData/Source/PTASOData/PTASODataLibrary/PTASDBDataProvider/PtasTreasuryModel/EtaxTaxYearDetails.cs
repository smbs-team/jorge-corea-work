using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasTreasuryModel
{
    public partial class EtaxTaxYearDetails
    {
        [Key]
        public string AccountNum { get; set; }
        public string ParcelNum { get; set; }
        public int RollYear { get; set; }
        public string BillingClassification { get; set; }
        public decimal? Amount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasTreasuryModel
{
    public partial class EtaxBillingSummary
    {        
        public string PaymentGroupId { get; set; }
        public string ParcelNum { get; set; }
        [Key]
        public string AccountNum { get; set; }
        public DateTime DueDate { get; set; }
        public int RollYear { get; set; }
        public string CartType { get; set; }
        public int HasPendingPayments { get; set; }
        public decimal? Amount { get; set; }
    }
}

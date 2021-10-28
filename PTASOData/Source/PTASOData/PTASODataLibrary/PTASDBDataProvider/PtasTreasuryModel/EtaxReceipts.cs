using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasTreasuryModel
{
    public partial class EtaxReceipts
    {
        public int TaxYear { get; set; }
        public string PaymentId { get; set; }
        public int Canceled { get; set; }
        public string AccountNum { get; set; }
        public DateTime PaymentDate { get; set; }
        [Key]
        public string ReceiptNum { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal? InterestAmount { get; set; }
    }
}

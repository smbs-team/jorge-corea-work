using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace PTASODataLibrary.PtasDbDataProvider.PtasTreasuryModel
{
    public partial class EtaxAccountMaster
    {
        [Key]
        public string CustomerAccount { get; set; }
        public string ParcelNum { get; set; }
        public int SplitCode { get; set; }
        public string LenderCompanyCode { get; set; }
        public int TaxAccountType { get; set; }
        public int PropertyType { get; set; }
        public string TaxPayerName { get; set; }
        public string CurrentMailingAddress { get; set; }
        public string AccountStatus { get; set; }
        public string SubjectToForeclosure { get; set; }
        public string Foreclosure { get; set; }
        public string LidssubjectToForeclosure { get; set; }
        public string Lidsforeclosure { get; set; }
        public string LidsorMams { get; set; }
        public string FiveYearOldOpen { get; set; }
    }
}

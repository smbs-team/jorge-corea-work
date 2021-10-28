using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_sales_parceldetail_parcelsinsale
    {
        public Guid ptas_sales_parceldetail_parcelsinsaleid { get; set; }
        public Guid? ptas_parceldetailid { get; set; }
        public Guid? ptas_salesid { get; set; }
        public long? versionnumber { get; set; }
        public DateTime modifiedon { get; set; }
    }
}

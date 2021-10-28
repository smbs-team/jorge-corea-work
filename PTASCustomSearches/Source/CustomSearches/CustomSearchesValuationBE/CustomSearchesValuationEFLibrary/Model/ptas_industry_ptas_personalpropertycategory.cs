using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_industry_ptas_personalpropertycategory
    {
        public Guid ptas_industry_ptas_personalpropertycategoryid { get; set; }
        public Guid? ptas_industryid { get; set; }
        public Guid? ptas_personalpropertycategoryid { get; set; }
        public long? versionnumber { get; set; }
    }
}

using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_county_stateorprovince
    {
        public Guid ptas_county_stateorprovinceid { get; set; }
        public Guid? ptas_countyid { get; set; }
        public Guid? ptas_stateorprovinceid { get; set; }
        public long? versionnumber { get; set; }
        public DateTime modifiedon { get; set; }
    }
}

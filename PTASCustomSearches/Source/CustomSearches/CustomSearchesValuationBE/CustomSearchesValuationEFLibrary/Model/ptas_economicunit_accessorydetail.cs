using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_economicunit_accessorydetail
    {
        public Guid ptas_economicunit_accessorydetailid { get; set; }
        public Guid? ptas_accessorydetailid { get; set; }
        public Guid? ptas_economicunitid { get; set; }
        public long? versionnumber { get; set; }
        public DateTime modifiedon { get; set; }
    }
}

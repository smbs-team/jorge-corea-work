﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_city_county
    {
        public Guid ptas_city_countyid { get; set; }
        public Guid? ptas_cityid { get; set; }
        public Guid? ptas_countyid { get; set; }
        public long? versionnumber { get; set; }
        public DateTime modifiedon { get; set; }
    }
}

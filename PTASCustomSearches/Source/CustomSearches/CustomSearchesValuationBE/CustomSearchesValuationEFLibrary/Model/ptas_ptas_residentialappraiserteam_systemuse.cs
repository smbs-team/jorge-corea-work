using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_ptas_residentialappraiserteam_systemuse
    {
        public Guid ptas_ptas_residentialappraiserteam_systemuseid { get; set; }
        public Guid? ptas_residentialappraiserteamid { get; set; }
        public Guid? systemuserid { get; set; }
        public long? versionnumber { get; set; }
    }
}

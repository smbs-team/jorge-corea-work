using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_salesnote_ptas_mediarepository
    {
        public Guid ptas_salesnote_ptas_mediarepositoryid { get; set; }
        public Guid? ptas_mediarepositoryid { get; set; }
        public Guid? ptas_salesnoteid { get; set; }
        public long? versionnumber { get; set; }
    }
}

using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_ptas_bookmark_ptas_bookmarktag
    {
        public Guid ptas_ptas_bookmark_ptas_bookmarktagid { get; set; }
        public Guid? ptas_bookmarkid { get; set; }
        public Guid? ptas_bookmarktagid { get; set; }
        public long? versionnumber { get; set; }
        public DateTime modifiedon { get; set; }
    }
}

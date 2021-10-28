using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_ptas_taxaccount_ptas_fileattachmentmeta
    {
        public Guid ptas_ptas_taxaccount_ptas_fileattachmentmetaid { get; set; }
        public Guid? ptas_fileattachmentmetadataid { get; set; }
        public Guid? ptas_taxaccountid { get; set; }
        public long? versionnumber { get; set; }
    }
}

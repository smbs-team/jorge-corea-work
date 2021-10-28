using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_ptas_camanotes_ptas_fileattachmentmetad
    {
        public Guid ptas_ptas_camanotes_ptas_fileattachmentmetadid { get; set; }
        public Guid? ptas_camanotesid { get; set; }
        public Guid? ptas_fileattachmentmetadataid { get; set; }
        public long? versionnumber { get; set; }
        public DateTime modifiedon { get; set; }
    }
}

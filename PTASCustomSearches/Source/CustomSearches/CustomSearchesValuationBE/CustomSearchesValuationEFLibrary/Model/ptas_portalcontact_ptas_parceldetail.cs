using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_portalcontact_ptas_parceldetail
    {
        public Guid ptas_portalcontact_ptas_parceldetailid { get; set; }
        public Guid? ptas_parceldetailid { get; set; }
        public Guid? ptas_portalcontactid { get; set; }
        public long? versionnumber { get; set; }
    }
}

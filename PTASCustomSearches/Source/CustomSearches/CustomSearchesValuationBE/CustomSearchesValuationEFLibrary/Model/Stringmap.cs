using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class stringmap
    {
        public Guid stringmapid { get; set; }
        public string attributename { get; set; }
        public long? attributevalue { get; set; }
        public int? displayorder { get; set; }
        public int? langid { get; set; }
        public string objecttypecode { get; set; }
        public string value { get; set; }
        public long? versionnumber { get; set; }
        public Guid? organizationid { get; set; }
        public DateTime modifiedon { get; set; }
    }
}

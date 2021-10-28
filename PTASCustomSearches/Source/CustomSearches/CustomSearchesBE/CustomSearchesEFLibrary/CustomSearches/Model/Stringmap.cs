using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class Stringmap
    {
        public Guid Stringmapid { get; set; }
        public string Attributename { get; set; }
        public long? Attributevalue { get; set; }
        public int? Displayorder { get; set; }
        public int? Langid { get; set; }
        public string Objecttypecode { get; set; }
        public string Value { get; set; }
        public long? Versionnumber { get; set; }
        public Guid? Organizationid { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}

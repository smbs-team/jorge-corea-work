using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class teamroles
    {
        public Guid teamroleid { get; set; }
        public Guid? roleid { get; set; }
        public Guid? teamid { get; set; }
        public long? versionnumber { get; set; }
        public DateTime modifiedon { get; set; }
    }
}

using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class Teammembership
    {
        public Guid Teammembershipid { get; set; }
        public Guid? Systemuserid { get; set; }
        public Guid? Teamid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}

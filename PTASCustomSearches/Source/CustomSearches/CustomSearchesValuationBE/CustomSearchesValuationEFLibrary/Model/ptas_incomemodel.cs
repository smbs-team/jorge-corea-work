using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_incomemodel
    {
        public Guid incomemodelId { get; set; }
        public string name { get; set; }
        public Guid? geoArea { get; set; }
        public Guid? geoNbhd { get; set; }
        public Guid? assessmentYear { get; set; }
        public Guid? specArea { get; set; }
        public Guid? specNbhd { get; set; }
        public int? readyForValuation { get; set; }
    }
}

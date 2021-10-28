using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class ChartType
    {
        public ChartType()
        {
            ChartTemplate = new HashSet<ChartTemplate>();
            InteractiveChart = new HashSet<InteractiveChart>();
        }

        public string ChartType1 { get; set; }

        public virtual ICollection<ChartTemplate> ChartTemplate { get; set; }
        public virtual ICollection<InteractiveChart> InteractiveChart { get; set; }
    }
}

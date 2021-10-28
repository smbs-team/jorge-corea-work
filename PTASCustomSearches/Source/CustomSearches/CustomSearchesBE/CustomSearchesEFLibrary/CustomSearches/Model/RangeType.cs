using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class RangeType
    {
        public RangeType()
        {
            CustomSearchParameter = new HashSet<CustomSearchParameter>();
        }

        public string RangeType1 { get; set; }

        public virtual ICollection<CustomSearchParameter> CustomSearchParameter { get; set; }
    }
}

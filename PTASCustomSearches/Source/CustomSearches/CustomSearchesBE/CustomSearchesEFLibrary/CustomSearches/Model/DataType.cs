using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class DataType
    {
        public DataType()
        {
            CustomSearchColumnDefinition = new HashSet<CustomSearchColumnDefinition>();
            CustomSearchParameter = new HashSet<CustomSearchParameter>();
        }

        public string DataType1 { get; set; }

        public virtual ICollection<CustomSearchColumnDefinition> CustomSearchColumnDefinition { get; set; }
        public virtual ICollection<CustomSearchParameter> CustomSearchParameter { get; set; }
    }
}

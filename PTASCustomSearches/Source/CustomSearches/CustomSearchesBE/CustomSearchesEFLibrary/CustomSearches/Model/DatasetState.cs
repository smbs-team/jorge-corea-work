using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class DatasetState
    {
        public DatasetState()
        {
            Dataset = new HashSet<Dataset>();
        }

        public string DatasetState1 { get; set; }

        public virtual ICollection<Dataset> Dataset { get; set; }
    }
}

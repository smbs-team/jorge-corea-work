using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class PostProcessType
    {
        public PostProcessType()
        {
            DatasetPostProcess = new HashSet<DatasetPostProcess>();
        }

        public string PostProcessType1 { get; set; }

        public virtual ICollection<DatasetPostProcess> DatasetPostProcess { get; set; }
    }
}

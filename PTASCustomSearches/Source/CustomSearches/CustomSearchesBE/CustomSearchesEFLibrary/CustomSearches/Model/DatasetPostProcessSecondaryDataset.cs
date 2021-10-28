using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class DatasetPostProcessSecondaryDataset
    {
        public int DatasetPostProcessId { get; set; }
        public Guid SecondaryDatasetId { get; set; }

        public virtual DatasetPostProcess DatasetPostProcess { get; set; }
        public virtual Dataset SecondaryDataset { get; set; }
    }
}

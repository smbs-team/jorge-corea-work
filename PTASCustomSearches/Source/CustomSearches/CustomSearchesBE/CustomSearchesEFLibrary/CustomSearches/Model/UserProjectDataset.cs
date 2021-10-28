using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class UserProjectDataset
    {
        public int UserProjectId { get; set; }
        public Guid DatasetId { get; set; }
        public bool OwnsDataset { get; set; }
        public string DatasetRole { get; set; }

        public virtual Dataset Dataset { get; set; }
        public virtual UserProject UserProject { get; set; }
    }
}

using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class DatasetUserClientState
    {
        public Guid UserId { get; set; }
        public Guid DatasetId { get; set; }
        public string DatasetClientState { get; set; }

        public virtual Dataset Dataset { get; set; }
    }
}

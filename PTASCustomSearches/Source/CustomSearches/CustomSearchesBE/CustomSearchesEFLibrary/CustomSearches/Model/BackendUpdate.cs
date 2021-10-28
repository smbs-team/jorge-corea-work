using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class BackendUpdate
    {
        public int BackendUpdateId { get; set; }
        public int? DatasetPostProcessId { get; set; }
        public string UpdatesJson { get; set; }
        public string ExportState { get; set; }
        public string ExportError { get; set; }
        public string SingleRowMajor { get; set; }
        public string SingleRowMinor { get; set; }

        public virtual DatasetPostProcess DatasetPostProcess { get; set; }
    }
}

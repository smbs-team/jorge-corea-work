using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_ptas_task_ptas_taxrollcorrection
    {
        public Guid ptas_ptas_task_ptas_taxrollcorrectionid { get; set; }
        public Guid? ptas_taskid { get; set; }
        public Guid? ptas_taxrollcorrectionid { get; set; }
        public long? versionnumber { get; set; }
    }
}

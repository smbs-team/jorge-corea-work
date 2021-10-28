using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasIndustryPtasPersonalpropertycategory
    {
        public Guid PtasIndustryPtasPersonalpropertycategoryid { get; set; }
        public Guid? PtasIndustryid { get; set; }
        public Guid? PtasPersonalpropertycategoryid { get; set; }
        public long? Versionnumber { get; set; }
    }
}

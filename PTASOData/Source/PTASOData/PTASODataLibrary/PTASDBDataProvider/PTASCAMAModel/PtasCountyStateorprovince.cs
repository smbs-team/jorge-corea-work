using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCountyStateorprovince
    {
        public Guid PtasCountyStateorprovinceid { get; set; }
        public Guid? PtasCountyid { get; set; }
        public Guid? PtasStateorprovinceid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}

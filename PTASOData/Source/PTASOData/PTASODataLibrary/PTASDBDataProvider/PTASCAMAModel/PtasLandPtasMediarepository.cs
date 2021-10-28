using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasLandPtasMediarepository
    {
        public Guid PtasLandPtasMediarepositoryid { get; set; }
        public Guid? PtasLandid { get; set; }
        public Guid? PtasMediarepositoryid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}

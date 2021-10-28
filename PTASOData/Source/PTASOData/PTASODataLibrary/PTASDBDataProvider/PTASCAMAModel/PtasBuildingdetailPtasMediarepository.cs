using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasBuildingdetailPtasMediarepository
    {
        public Guid PtasBuildingdetailPtasMediarepositoryid { get; set; }
        public Guid? PtasBuildingdetailid { get; set; }
        public Guid? PtasMediarepositoryid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}

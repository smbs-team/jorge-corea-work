using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPtasTaskPtasParceldetail
    {
        public Guid PtasPtasTaskPtasParceldetailid { get; set; }
        public Guid? PtasParceldetailid { get; set; }
        public Guid? PtasTaskid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}

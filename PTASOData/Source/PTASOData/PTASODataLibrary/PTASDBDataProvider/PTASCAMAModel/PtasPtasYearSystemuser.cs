using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPtasYearSystemuser
    {
        public Guid PtasPtasYearSystemuserid { get; set; }
        public Guid? PtasYearid { get; set; }
        public Guid? Systemuserid { get; set; }
        public long? Versionnumber { get; set; }
    }
}

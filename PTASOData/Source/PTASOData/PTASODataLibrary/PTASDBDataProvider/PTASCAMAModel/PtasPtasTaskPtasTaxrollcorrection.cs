using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasPtasTaskPtasTaxrollcorrection
    {
        public Guid PtasPtasTaskPtasTaxrollcorrectionid { get; set; }
        public Guid? PtasTaskid { get; set; }
        public Guid? PtasTaxrollcorrectionid { get; set; }
        public long? Versionnumber { get; set; }
    }
}

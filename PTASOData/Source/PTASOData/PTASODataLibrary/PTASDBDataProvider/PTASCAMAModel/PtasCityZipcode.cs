using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCityZipcode
    {
        public Guid PtasCityZipcodeid { get; set; }
        public Guid? PtasCityid { get; set; }
        public Guid? PtasZipcodeid { get; set; }
        public long? Versionnumber { get; set; }
        public DateTime Modifiedon { get; set; }
    }
}

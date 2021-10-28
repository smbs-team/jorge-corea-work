using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasTreasuryModel
{
    public partial class IntegrationLog
    {
        public string IntegrationName { get; set; }
        public int? BatchId { get; set; }
        public string State { get; set; }
        public string Comment { get; set; }
        public DateTime? CreatedTimeStamp { get; set; }
        public DateTime? CompletedTimeStamp { get; set; }
        public int Id { get; set; }
    }
}

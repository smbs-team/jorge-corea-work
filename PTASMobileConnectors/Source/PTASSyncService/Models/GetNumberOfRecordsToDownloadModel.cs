using System;

namespace PTASSyncService.Models
{
    public class GetNumberOfRecordsToDownloadModel
    {
        public Nullable<int> NumRecs { get; set; }
        public string SourceTable { get; set; }
        public Nullable<long> downloadId { get; set; }
        public Nullable<double> syncDate { get; set; }
    }
}

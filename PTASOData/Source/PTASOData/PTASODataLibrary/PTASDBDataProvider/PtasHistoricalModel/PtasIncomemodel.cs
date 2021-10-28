using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PTASODataLibrary.PtasDbDataProvider.PtasHistoricalModel
{
    public partial class PtasIncomemodel
    {
        [Key]
        public Guid IncomemodelId { get; set; }
        public string Name { get; set; }
        public Guid? GeoArea { get; set; }
        public Guid? GeoNbhd { get; set; }
        public Guid? AssessmentYear { get; set; }
        public Guid? SpecArea { get; set; }
        public Guid? SpecNbhd { get; set; }
        public int? ReadyForValuation { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasZoning
    {
        public PtasZoning()
        {
            PtasAbstractproject = new HashSet<PtasAbstractproject>();
            PtasLandvaluecalculation = new HashSet<PtasLandvaluecalculation>();
            PtasSalesaggregate = new HashSet<PtasSalesaggregate>();
        }

        public Guid PtasZoningid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasName { get; set; }
        public int? PtasZoneid { get; set; }
        public int? PtasZoningcode { get; set; }
        public string PtasZoningdescription { get; set; }
        public int? Statecode { get; set; }
        public int? Statuscode { get; set; }
        public int? Timezoneruleversionnumber { get; set; }
        public int? Utcconversiontimezonecode { get; set; }
        public long? Versionnumber { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? OrganizationidValue { get; set; }
        public Guid? PtasTaxdistrictidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasTaxdistrictidValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractproject { get; set; }
        public virtual ICollection<PtasLandvaluecalculation> PtasLandvaluecalculation { get; set; }
        public virtual ICollection<PtasSalesaggregate> PtasSalesaggregate { get; set; }
    }
}

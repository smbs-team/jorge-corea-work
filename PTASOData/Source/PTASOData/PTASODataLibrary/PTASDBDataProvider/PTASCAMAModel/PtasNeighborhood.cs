using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasNeighborhood
    {
        public PtasNeighborhood()
        {
            PtasAbstractproject = new HashSet<PtasAbstractproject>();
            PtasAbstractprojectresultparcel = new HashSet<PtasAbstractprojectresultparcel>();
            PtasAptlistedrent = new HashSet<PtasAptlistedrent>();
            PtasAptneighborhood = new HashSet<PtasAptneighborhood>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
        }

        public Guid PtasNeighborhoodid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasDescription { get; set; }
        public string PtasName { get; set; }
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
        public Guid? PtasAreaidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual PtasArea PtasAreaidValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractproject { get; set; }
        public virtual ICollection<PtasAbstractprojectresultparcel> PtasAbstractprojectresultparcel { get; set; }
        public virtual ICollection<PtasAptlistedrent> PtasAptlistedrent { get; set; }
        public virtual ICollection<PtasAptneighborhood> PtasAptneighborhood { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetail { get; set; }
    }
}

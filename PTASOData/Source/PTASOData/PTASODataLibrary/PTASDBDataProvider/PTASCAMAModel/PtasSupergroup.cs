using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSupergroup
    {
        public PtasSupergroup()
        {
            PtasAptincomeexpense = new HashSet<PtasAptincomeexpense>();
            PtasAptvaluation = new HashSet<PtasAptvaluation>();
            PtasGeoneighborhood = new HashSet<PtasGeoneighborhood>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
            PtasSpecialtyneighborhood = new HashSet<PtasSpecialtyneighborhood>();
        }

        public Guid PtasSupergroupid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasName { get; set; }
        public string PtasSupergroupnumber { get; set; }
        public int? PtasSupergrouptype { get; set; }
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

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual ICollection<PtasAptincomeexpense> PtasAptincomeexpense { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluation { get; set; }
        public virtual ICollection<PtasGeoneighborhood> PtasGeoneighborhood { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetail { get; set; }
        public virtual ICollection<PtasSpecialtyneighborhood> PtasSpecialtyneighborhood { get; set; }
    }
}

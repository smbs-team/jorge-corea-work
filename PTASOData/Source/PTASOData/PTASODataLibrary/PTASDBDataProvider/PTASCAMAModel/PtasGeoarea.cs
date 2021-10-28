using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasGeoarea
    {
        public PtasGeoarea()
        {
            PtasAptvaluation = new HashSet<PtasAptvaluation>();
            PtasGeoneighborhood = new HashSet<PtasGeoneighborhood>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
            PtasSalesaggregate = new HashSet<PtasSalesaggregate>();
        }

        public Guid PtasGeoareaid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAreanumber { get; set; }
        public int? PtasCommercialdistrict { get; set; }
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
        public Guid? PtasAppraiseridValue { get; set; }
        public Guid? PtasSeniorappraiseridValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser PtasAppraiseridValueNavigation { get; set; }
        public virtual Systemuser PtasSeniorappraiseridValueNavigation { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluation { get; set; }
        public virtual ICollection<PtasGeoneighborhood> PtasGeoneighborhood { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetail { get; set; }
        public virtual ICollection<PtasSalesaggregate> PtasSalesaggregate { get; set; }
    }
}

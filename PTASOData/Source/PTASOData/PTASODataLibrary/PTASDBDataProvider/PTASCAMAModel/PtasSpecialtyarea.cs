using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSpecialtyarea
    {
        public PtasSpecialtyarea()
        {
            PtasAptavailablecomparablesale = new HashSet<PtasAptavailablecomparablesale>();
            PtasAptvaluation = new HashSet<PtasAptvaluation>();
            PtasBuildingdetailCommercialuse = new HashSet<PtasBuildingdetailCommercialuse>();
            PtasCondounit = new HashSet<PtasCondounit>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
            PtasSalesaggregate = new HashSet<PtasSalesaggregate>();
            PtasSpecialtyneighborhood = new HashSet<PtasSpecialtyneighborhood>();
        }

        public Guid PtasSpecialtyareaid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAreanumber { get; set; }
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
        public Guid? PtasSeniorappraiseridValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser PtasSeniorappraiseridValueNavigation { get; set; }
        public virtual ICollection<PtasAptavailablecomparablesale> PtasAptavailablecomparablesale { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluation { get; set; }
        public virtual ICollection<PtasBuildingdetailCommercialuse> PtasBuildingdetailCommercialuse { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounit { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetail { get; set; }
        public virtual ICollection<PtasSalesaggregate> PtasSalesaggregate { get; set; }
        public virtual ICollection<PtasSpecialtyneighborhood> PtasSpecialtyneighborhood { get; set; }
    }
}

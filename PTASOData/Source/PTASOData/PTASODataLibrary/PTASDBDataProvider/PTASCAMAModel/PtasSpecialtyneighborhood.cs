using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSpecialtyneighborhood
    {
        public PtasSpecialtyneighborhood()
        {
            PtasBuildingdetailCommercialuse = new HashSet<PtasBuildingdetailCommercialuse>();
            PtasCondounit = new HashSet<PtasCondounit>();
            PtasGradestratificationmapping = new HashSet<PtasGradestratificationmapping>();
            PtasInspectionyear = new HashSet<PtasInspectionyear>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
            PtasSalesaggregate = new HashSet<PtasSalesaggregate>();
        }

        public Guid PtasSpecialtyneighborhoodid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasDescription { get; set; }
        public string PtasName { get; set; }
        public string PtasNbhdnumber { get; set; }
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
        public Guid? PtasSpecialtyareaidValue { get; set; }
        public Guid? PtasSupergroupidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser PtasAppraiseridValueNavigation { get; set; }
        public virtual PtasSpecialtyarea PtasSpecialtyareaidValueNavigation { get; set; }
        public virtual PtasSupergroup PtasSupergroupidValueNavigation { get; set; }
        public virtual ICollection<PtasBuildingdetailCommercialuse> PtasBuildingdetailCommercialuse { get; set; }
        public virtual ICollection<PtasCondounit> PtasCondounit { get; set; }
        public virtual ICollection<PtasGradestratificationmapping> PtasGradestratificationmapping { get; set; }
        public virtual ICollection<PtasInspectionyear> PtasInspectionyear { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetail { get; set; }
        public virtual ICollection<PtasSalesaggregate> PtasSalesaggregate { get; set; }
    }
}

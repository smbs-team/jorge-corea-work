using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasBuildingsectionuse
    {
        public PtasBuildingsectionuse()
        {
            PtasBuildingdetail = new HashSet<PtasBuildingdetail>();
            PtasBuildingdetailCommercialuse = new HashSet<PtasBuildingdetailCommercialuse>();
            PtasSalesaggregate = new HashSet<PtasSalesaggregate>();
            PtasSectionusesqft = new HashSet<PtasSectionusesqft>();
        }

        public Guid PtasBuildingsectionuseid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAbbreviation { get; set; }
        public string PtasItemid { get; set; }
        public int? PtasMainframecode { get; set; }
        public string PtasMarshallswiftdescription { get; set; }
        public string PtasName { get; set; }
        public string PtasTypeid { get; set; }
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
        public virtual ICollection<PtasBuildingdetail> PtasBuildingdetail { get; set; }
        public virtual ICollection<PtasBuildingdetailCommercialuse> PtasBuildingdetailCommercialuse { get; set; }
        public virtual ICollection<PtasSalesaggregate> PtasSalesaggregate { get; set; }
        public virtual ICollection<PtasSectionusesqft> PtasSectionusesqft { get; set; }
    }
}

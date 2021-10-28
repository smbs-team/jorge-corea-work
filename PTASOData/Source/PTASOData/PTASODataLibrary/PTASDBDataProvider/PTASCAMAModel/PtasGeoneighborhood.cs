using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasGeoneighborhood
    {
        public PtasGeoneighborhood()
        {
            PtasAptvaluation = new HashSet<PtasAptvaluation>();
            PtasInspectionyear = new HashSet<PtasInspectionyear>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
            PtasSalesaggregate = new HashSet<PtasSalesaggregate>();
        }

        public Guid PtasGeoneighborhoodid { get; set; }
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
        public Guid? PtasGeoareaidValue { get; set; }
        public Guid? PtasSubmarketValue { get; set; }
        public Guid? PtasSupergroupValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual PtasGeoarea PtasGeoareaidValueNavigation { get; set; }
        public virtual PtasSubmarket PtasSubmarketValueNavigation { get; set; }
        public virtual PtasSupergroup PtasSupergroupValueNavigation { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluation { get; set; }
        public virtual ICollection<PtasInspectionyear> PtasInspectionyear { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetail { get; set; }
        public virtual ICollection<PtasSalesaggregate> PtasSalesaggregate { get; set; }
    }
}

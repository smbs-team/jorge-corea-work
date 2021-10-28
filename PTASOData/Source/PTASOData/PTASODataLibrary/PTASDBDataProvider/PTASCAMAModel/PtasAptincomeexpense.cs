using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAptincomeexpense
    {
        public Guid PtasAptincomeexpenseid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? PtasAssessmentyear { get; set; }
        public decimal? PtasBasecaprate { get; set; }
        public decimal? PtasBaseexpense { get; set; }
        public decimal? PtasBaseexpenseBase { get; set; }
        public decimal? PtasBasegim { get; set; }
        public decimal? PtasBasepercentexpense { get; set; }
        public decimal? PtasCommercialgim { get; set; }
        public int? PtasMaxnumberofunits { get; set; }
        public int? PtasMaxyearbuilt { get; set; }
        public int? PtasMinnumberofunits { get; set; }
        public int? PtasMinyearbuilt { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasOtherincome { get; set; }
        public decimal? PtasOtherincomeBase { get; set; }
        public int? PtasRegion { get; set; }
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
        public Guid? PtasApartmentneighborhoodidValue { get; set; }
        public Guid? PtasAssessmentyearlookupidValue { get; set; }
        public Guid? PtasSupergroupidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual PtasAptneighborhood PtasApartmentneighborhoodidValueNavigation { get; set; }
        public virtual PtasYear PtasAssessmentyearlookupidValueNavigation { get; set; }
        public virtual PtasSupergroup PtasSupergroupidValueNavigation { get; set; }
    }
}

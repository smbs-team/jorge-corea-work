using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAgriculturalusetype
    {
        public PtasAgriculturalusetype()
        {
            PtasCurrentuselanduse = new HashSet<PtasCurrentuselanduse>();
        }

        public Guid PtasAgriculturalusetypeid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? PtasAgriculturalincomelevel { get; set; }
        public int? PtasAgriculturalusecode { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasYearlyrent { get; set; }
        public decimal? PtasYearlyrentBase { get; set; }
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
        public Guid? PtasTaxyearValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual PtasYear PtasTaxyearValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuselanduse> PtasCurrentuselanduse { get; set; }
    }
}

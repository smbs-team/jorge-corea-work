using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCurrentusefarmyieldhistory
    {
        public Guid PtasCurrentusefarmyieldhistoryid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasGrossincomeperacre { get; set; }
        public decimal? PtasGrossincomeperacreBase { get; set; }
        public decimal? PtasGrossrentalfeeperacre { get; set; }
        public decimal? PtasGrossrentalfeeperacreBase { get; set; }
        public decimal? PtasInvestmentperacre { get; set; }
        public decimal? PtasInvestmentperacreBase { get; set; }
        public string PtasName { get; set; }
        public int? PtasYieldmeasurement { get; set; }
        public int? PtasYieldperacre { get; set; }
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
        public Guid? PtasCurrentuseapplicationValue { get; set; }
        public Guid? PtasYearValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual PtasCurrentuseapplication PtasCurrentuseapplicationValueNavigation { get; set; }
        public virtual PtasYear PtasYearValueNavigation { get; set; }
    }
}

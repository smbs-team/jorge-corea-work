using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCurrentuselanduse
    {
        public Guid PtasCurrentuselanduseid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAcres { get; set; }
        public decimal? PtasCapratesum { get; set; }
        public decimal? PtasCurrentuselandvalue { get; set; }
        public decimal? PtasCurrentuselandvalueBase { get; set; }
        public int? PtasLandusetype { get; set; }
        public string PtasName { get; set; }
        public int? PtasTaxreductionpercentage { get; set; }
        public decimal? PtasValueperacre { get; set; }
        public decimal? PtasValueperacreBase { get; set; }
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
        public Guid? PtasAgriculturalusetypeValue { get; set; }
        public Guid? PtasCurrentuseapplicationValue { get; set; }
        public Guid? PtasCurrentuseparcelidValue { get; set; }
        public Guid? PtasParcelValue { get; set; }
        public Guid? PtasTimberusetypeValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual PtasAgriculturalusetype PtasAgriculturalusetypeValueNavigation { get; set; }
        public virtual PtasCurrentuseapplication PtasCurrentuseapplicationValueNavigation { get; set; }
        public virtual PtasCurrentuseapplicationparcel PtasCurrentuseparcelidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelValueNavigation { get; set; }
        public virtual PtasTimberusetype PtasTimberusetypeValueNavigation { get; set; }
    }
}

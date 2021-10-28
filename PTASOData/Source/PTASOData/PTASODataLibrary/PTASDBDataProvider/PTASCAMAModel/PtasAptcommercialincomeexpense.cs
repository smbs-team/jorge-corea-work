using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAptcommercialincomeexpense
    {
        public Guid PtasAptcommercialincomeexpenseid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasCaprate { get; set; }
        public decimal? PtasEffectivegrossincome { get; set; }
        public decimal? PtasEffectivegrossincomeBase { get; set; }
        public decimal? PtasIncomevalue { get; set; }
        public decimal? PtasIncomevalueBase { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasNetoperatingincome { get; set; }
        public decimal? PtasNetoperatingincomeBase { get; set; }
        public int? PtasNetsqft { get; set; }
        public decimal? PtasOperatingexpensefactor { get; set; }
        public decimal? PtasOperatingexpenses { get; set; }
        public decimal? PtasOperatingexpensesBase { get; set; }
        public decimal? PtasPotentialgrossincome { get; set; }
        public decimal? PtasPotentialgrossincomeBase { get; set; }
        public decimal? PtasRentrate { get; set; }
        public decimal? PtasRentrateBase { get; set; }
        public decimal? PtasVacancyandcreditloss { get; set; }
        public int? Statecode { get; set; }
        public int? Statuscode { get; set; }
        public int? Timezoneruleversionnumber { get; set; }
        public int? Utcconversiontimezonecode { get; set; }
        public long? Versionnumber { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? OwneridValue { get; set; }
        public Guid? OwningbusinessunitValue { get; set; }
        public Guid? OwningteamValue { get; set; }
        public Guid? OwninguserValue { get; set; }
        public Guid? PtasAptvaluationidValue { get; set; }
        public Guid? PtasSectionuseidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasAptvaluation PtasAptvaluationidValueNavigation { get; set; }
        public virtual PtasBuildingdetailCommercialuse PtasSectionuseidValueNavigation { get; set; }
    }
}

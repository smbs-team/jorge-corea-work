using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasCurrentusevaluehistory
    {
        public Guid PtasCurrentusevaluehistoryid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAssessedimprovementsvalue { get; set; }
        public decimal? PtasAssessedimprovementsvalueBase { get; set; }
        public decimal? PtasAssessedlandvalue { get; set; }
        public decimal? PtasAssessedlandvalueBase { get; set; }
        public decimal? PtasAssessedtotalvalue { get; set; }
        public decimal? PtasAssessedtotalvalueBase { get; set; }
        public decimal? PtasCuexcludedareavalue { get; set; }
        public decimal? PtasCuexcludedareavalueBase { get; set; }
        public decimal? PtasCuexcludedreaacres { get; set; }
        public decimal? PtasCufarmagricultureacres { get; set; }
        public decimal? PtasCufarmagriculturelandvalue { get; set; }
        public decimal? PtasCufarmagriculturelandvalueBase { get; set; }
        public decimal? PtasCuforestacres { get; set; }
        public decimal? PtasCuforestlandvalue { get; set; }
        public decimal? PtasCuforestlandvalueBase { get; set; }
        public decimal? PtasCuhomesiteacres { get; set; }
        public decimal? PtasCuhomesitesvalue { get; set; }
        public decimal? PtasCuhomesitesvalueBase { get; set; }
        public decimal? PtasCuimprovementsvalue { get; set; }
        public decimal? PtasCuimprovementsvalueBase { get; set; }
        public decimal? PtasCulandvalue { get; set; }
        public decimal? PtasCulandvalueBase { get; set; }
        public decimal? PtasCuopenspaceacres { get; set; }
        public decimal? PtasCuopenspacelandvalue { get; set; }
        public decimal? PtasCuopenspacelandvalueBase { get; set; }
        public int? PtasCurrentusestatus { get; set; }
        public decimal? PtasCutimberacres { get; set; }
        public decimal? PtasCutimberlandvalue { get; set; }
        public decimal? PtasCutimberlandvalueBase { get; set; }
        public DateTimeOffset? PtasEffectivedate { get; set; }
        public string PtasName { get; set; }
        public bool? PtasOverrideassessedvalues { get; set; }
        public decimal? PtasTotalcuacres { get; set; }
        public decimal? PtasTotalcuvalue { get; set; }
        public decimal? PtasTotalcuvalueBase { get; set; }
        public decimal? PtasTotalparcelacres { get; set; }
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
        public Guid? PtasCurrentuseparcelidValue { get; set; }
        public Guid? PtasLevycodeidValue { get; set; }
        public Guid? PtasTaxyearidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasCurrentuseapplicationparcel PtasCurrentuseparcelidValueNavigation { get; set; }
        public virtual PtasLevycode PtasLevycodeidValueNavigation { get; set; }
        public virtual PtasYear PtasTaxyearidValueNavigation { get; set; }
    }
}

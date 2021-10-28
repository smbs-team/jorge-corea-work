using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAnnexationparcelreview
    {
        public Guid PtasAnnexationparcelreviewid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAssessedimprovements { get; set; }
        public decimal? PtasAssessedimprovementsBase { get; set; }
        public decimal? PtasAssessedland { get; set; }
        public decimal? PtasAssessedlandBase { get; set; }
        public string PtasName { get; set; }
        public string PtasPropertyowner { get; set; }
        public bool? PtasSignedpetition { get; set; }
        public decimal? PtasTotalassessedvalue { get; set; }
        public decimal? PtasTotalassessedvalueBase { get; set; }
        public bool? PtasVerifiedpropertyowner { get; set; }
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
        public Guid? PtasAnnexationtrackeridValue { get; set; }
        public Guid? PtasLevycodeidValue { get; set; }
        public Guid? PtasParcelValue { get; set; }
        public Guid? PtasTaxrollyearforavValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasAnnexationtracker PtasAnnexationtrackeridValueNavigation { get; set; }
        public virtual PtasLevycode PtasLevycodeidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelValueNavigation { get; set; }
        public virtual PtasYear PtasTaxrollyearforavValueNavigation { get; set; }
    }
}

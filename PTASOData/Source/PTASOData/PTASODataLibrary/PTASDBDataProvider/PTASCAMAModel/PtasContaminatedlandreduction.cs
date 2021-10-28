using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasContaminatedlandreduction
    {
        public Guid PtasContaminatedlandreductionid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAssessedvaluereduction { get; set; }
        public decimal? PtasAssessedvaluereductionBase { get; set; }
        public decimal? PtasBaselandvalue { get; set; }
        public decimal? PtasBaselandvalueBase { get; set; }
        public decimal? PtasImprovementvalue { get; set; }
        public decimal? PtasImprovementvalueBase { get; set; }
        public decimal? PtasLandreducedvalue { get; set; }
        public decimal? PtasLandreducedvalueBase { get; set; }
        public decimal? PtasLandreducedvaluerounded { get; set; }
        public decimal? PtasLandreducedvalueroundedBase { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasPercentremediationcost { get; set; }
        public decimal? PtasPresentcost { get; set; }
        public decimal? PtasPresentcostBase { get; set; }
        public decimal? PtasTotalvalue { get; set; }
        public decimal? PtasTotalvalueBase { get; set; }
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
        public Guid? PtasAssessmentyearidValue { get; set; }
        public Guid? PtasContaminatedprojectidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasYear PtasAssessmentyearidValueNavigation { get; set; }
        public virtual PtasContaminationproject PtasContaminatedprojectidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
    }
}

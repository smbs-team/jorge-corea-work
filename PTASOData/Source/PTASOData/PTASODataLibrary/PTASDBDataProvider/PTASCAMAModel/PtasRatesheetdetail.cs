using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasRatesheetdetail
    {
        public PtasRatesheetdetail()
        {
            PtasFundfactordetail = new HashSet<PtasFundfactordetail>();
        }

        public Guid PtasRatesheetdetailid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? Ptas100tav { get; set; }
        public decimal? Ptas100tavBase { get; set; }
        public decimal? PtasActuallidlift { get; set; }
        public decimal? PtasActuallidliftBase { get; set; }
        public decimal? PtasAllowablelidlift { get; set; }
        public decimal? PtasAllowablelidliftBase { get; set; }
        public decimal? PtasBondorcapitalexcesslevyav { get; set; }
        public decimal? PtasBondorcapitalexcesslevyavBase { get; set; }
        public decimal? PtasBondrate { get; set; }
        public decimal? PtasBondrequestedamount { get; set; }
        public decimal? PtasBondrequestedamountBase { get; set; }
        public decimal? PtasCapitalprojectslevyamount { get; set; }
        public decimal? PtasCapitalprojectslevyamountBase { get; set; }
        public decimal? PtasCapitalprojectslevyrate { get; set; }
        public bool? PtasCertified { get; set; }
        public decimal? PtasCertifiedactuallidlift { get; set; }
        public decimal? PtasCertifiedactuallidliftBase { get; set; }
        public decimal? PtasCertifiedlidliftorbondrate { get; set; }
        public decimal? PtasCertifiedtotalrate { get; set; }
        public bool? PtasConsolidatedlevy { get; set; }
        public decimal? PtasDollarincreaselimitfactor { get; set; }
        public decimal? PtasDollarincreaselimitfactorBase { get; set; }
        public decimal? PtasExcesslevyassessedvalue { get; set; }
        public decimal? PtasExcesslevyassessedvalueBase { get; set; }
        public bool? PtasFirstyear { get; set; }
        public decimal? PtasFirstyearamt { get; set; }
        public decimal? PtasFirstyearamtBase { get; set; }
        public decimal? PtasHalfor80tav { get; set; }
        public decimal? PtasHalfor80tavBase { get; set; }
        public decimal? PtasLidliftbondamount { get; set; }
        public decimal? PtasLidliftbondamountBase { get; set; }
        public decimal? PtasLidliftorbondrate { get; set; }
        public decimal? PtasLimitfactor { get; set; }
        public decimal? PtasLocalnewconstruction { get; set; }
        public decimal? PtasLocalnewconstructionBase { get; set; }
        public decimal? PtasMaxlimitfactor { get; set; }
        public decimal? PtasMoexcesslevyav { get; set; }
        public decimal? PtasMoexcesslevyavBase { get; set; }
        public decimal? PtasMolevyrate { get; set; }
        public decimal? PtasMolevyrequestedamount { get; set; }
        public decimal? PtasMolevyrequestedamountBase { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasNewconstructioneffectonlidlift { get; set; }
        public decimal? PtasNewconstructioneffectonlidliftBase { get; set; }
        public decimal? PtasPrioryearallowable { get; set; }
        public decimal? PtasPrioryearallowableBase { get; set; }
        public decimal? PtasPrioryearallowablewithlimitfactor { get; set; }
        public decimal? PtasPrioryearallowablewithlimitfactorBase { get; set; }
        public decimal? PtasPrioryearallowablewithmaxlimitfactor { get; set; }
        public decimal? PtasPrioryearallowablewithmaxlimitfactorBase { get; set; }
        public decimal? PtasPrioryearlidliftorbondrate { get; set; }
        public int? PtasRateoramount { get; set; }
        public int? PtasRatesheettype { get; set; }
        public decimal? PtasRegularlevyassessedvalue { get; set; }
        public decimal? PtasRegularlevyassessedvalueBase { get; set; }
        public bool? PtasSchoolcertified { get; set; }
        public decimal? PtasTotallimitedlidlift { get; set; }
        public decimal? PtasTotallimitedlidliftBase { get; set; }
        public decimal? PtasTotalrate { get; set; }
        public decimal? PtasTransportationlevyamount { get; set; }
        public decimal? PtasTransportationlevyamountBase { get; set; }
        public decimal? PtasTransportationlevyrate { get; set; }
        public decimal? PtasUtilitynewconstruction { get; set; }
        public decimal? PtasUtilitynewconstructionBase { get; set; }
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
        public Guid? PtasLevylidliftbondidValue { get; set; }
        public Guid? PtasLevylimitworksheetdetailidValue { get; set; }
        public Guid? PtasLevyordinancerequestidValue { get; set; }
        public Guid? PtasTaxdistrictidValue { get; set; }
        public Guid? PtasYearidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasLevylidliftbond PtasLevylidliftbondidValueNavigation { get; set; }
        public virtual PtasLevyworksheetdetails PtasLevylimitworksheetdetailidValueNavigation { get; set; }
        public virtual PtasLevyordinancerequest PtasLevyordinancerequestidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasTaxdistrictidValueNavigation { get; set; }
        public virtual PtasYear PtasYearidValueNavigation { get; set; }
        public virtual ICollection<PtasFundfactordetail> PtasFundfactordetail { get; set; }
    }
}

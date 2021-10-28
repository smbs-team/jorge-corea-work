using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasLevyordinancerequest
    {
        public PtasLevyordinancerequest()
        {
            PtasRatesheetdetail = new HashSet<PtasRatesheetdetail>();
        }

        public Guid PtasLevyordinancerequestid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAllowablereglevyrequest { get; set; }
        public decimal? PtasAllowablereglevyrequestBase { get; set; }
        public decimal? PtasDollarincreaselimitfactor { get; set; }
        public decimal? PtasDollarincreaselimitfactorBase { get; set; }
        public decimal? PtasDollarincreaseoverlastyearsactuallevy { get; set; }
        public decimal? PtasDollarincreaseoverlastyearsactuallevyBase { get; set; }
        public decimal? PtasFirstyearamounts { get; set; }
        public decimal? PtasFirstyearamountsBase { get; set; }
        public decimal? PtasLastyearsactuallevy { get; set; }
        public decimal? PtasLastyearsactuallevyBase { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasPercentincreaseoverlastyearsactuallevy { get; set; }
        public decimal? PtasRefund { get; set; }
        public decimal? PtasRefundBase { get; set; }
        public decimal? PtasReglevyamt { get; set; }
        public decimal? PtasReglevyamtBase { get; set; }
        public decimal? PtasRegularlevyrequested { get; set; }
        public decimal? PtasRegularlevyrequestedBase { get; set; }
        public decimal? PtasTotalamtrequested { get; set; }
        public decimal? PtasTotalamtrequestedBase { get; set; }
        public decimal? PtasTotaldollaroverlastyearsactuallevy { get; set; }
        public decimal? PtasTotaldollaroverlastyearsactuallevyBase { get; set; }
        public decimal? PtasTotalgobonds { get; set; }
        public decimal? PtasTotalgobondsBase { get; set; }
        public decimal? PtasTotallidliftincreases { get; set; }
        public decimal? PtasTotallidliftincreasesBase { get; set; }
        public decimal? PtasTotallimitedbonds { get; set; }
        public decimal? PtasTotallimitedbondsBase { get; set; }
        public decimal? PtasTotalpercentoverlastyearsactuallevy { get; set; }
        public decimal? PtasTotalpercentoverlastyearsactuallevyBase { get; set; }
        public decimal? PtasTotalregularlevy { get; set; }
        public decimal? PtasTotalregularlevyBase { get; set; }
        public decimal? PtasTotalreqreglevies { get; set; }
        public decimal? PtasTotalreqregleviesBase { get; set; }
        public decimal? PtasTotalrequestedlidlifts { get; set; }
        public decimal? PtasTotalrequestedlidliftsBase { get; set; }
        public decimal? PtasTotalrequestedreserve { get; set; }
        public decimal? PtasTotalrequestedreserveBase { get; set; }
        public decimal? PtasTotalrequestedspeciallevies { get; set; }
        public decimal? PtasTotalrequestedspecialleviesBase { get; set; }
        public decimal? PtasTotaltaxesrequested { get; set; }
        public decimal? PtasTotaltaxesrequestedBase { get; set; }
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
        public Guid? PtasLevylimitworksheetdetailidValue { get; set; }
        public Guid? PtasTaxdistrictidValue { get; set; }
        public Guid? PtasYearidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasLevyworksheetdetails PtasLevylimitworksheetdetailidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasTaxdistrictidValueNavigation { get; set; }
        public virtual PtasYear PtasYearidValueNavigation { get; set; }
        public virtual ICollection<PtasRatesheetdetail> PtasRatesheetdetail { get; set; }
    }
}

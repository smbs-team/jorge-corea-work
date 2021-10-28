using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasLevylidliftbond
    {
        public PtasLevylidliftbond()
        {
            PtasFund = new HashSet<PtasFund>();
            PtasRatesheetdetail = new HashSet<PtasRatesheetdetail>();
        }

        public Guid PtasLevylidliftbondid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasCumulativeamount { get; set; }
        public decimal? PtasCumulativeamountBase { get; set; }
        public bool? PtasFarmequipmentexempt { get; set; }
        public decimal? PtasMaximumamount { get; set; }
        public decimal? PtasMaximumamountBase { get; set; }
        public decimal? PtasMaximumlimitfactor { get; set; }
        public string PtasName { get; set; }
        public bool? PtasSeniorexempt { get; set; }
        public int? PtasType { get; set; }
        public bool? PtasUseexcesslevy { get; set; }
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
        public Guid? PtasFirstyearidValue { get; set; }
        public Guid? PtasLastyearidValue { get; set; }
        public Guid? PtasTaxdistrictidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasYear PtasFirstyearidValueNavigation { get; set; }
        public virtual PtasYear PtasLastyearidValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasTaxdistrictidValueNavigation { get; set; }
        public virtual ICollection<PtasFund> PtasFund { get; set; }
        public virtual ICollection<PtasRatesheetdetail> PtasRatesheetdetail { get; set; }
    }
}

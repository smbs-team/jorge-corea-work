using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasBillingcode
    {
        public PtasBillingcode()
        {
            PtasFundfactordetail = new HashSet<PtasFundfactordetail>();
        }

        public Guid PtasBillingcodeid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public bool? PtasAllowamountchanges { get; set; }
        public bool? PtasAllowledgeraccountchanges { get; set; }
        public bool? PtasApproved { get; set; }
        public int? PtasBillingbasedon { get; set; }
        public string PtasBillingcodeattribute { get; set; }
        public int? PtasBillingcodedetermines { get; set; }
        public bool? PtasBillwhenzero { get; set; }
        public string PtasDescription { get; set; }
        public DateTimeOffset? PtasEffectivedate { get; set; }
        public int? PtasLevycodetype { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasRate { get; set; }
        public bool? PtasRaterollyearspecific { get; set; }
        public int? PtasRatetype { get; set; }
        public DateTime? PtasValidfrom { get; set; }
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
        public Guid? PtasBillingclassificationidValue { get; set; }
        public Guid? PtasFundidValue { get; set; }
        public Guid? PtasLevycodeidValue { get; set; }
        public Guid? PtasYearidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasBillingclassification PtasBillingclassificationidValueNavigation { get; set; }
        public virtual PtasFund PtasFundidValueNavigation { get; set; }
        public virtual PtasLevycode PtasLevycodeidValueNavigation { get; set; }
        public virtual PtasYear PtasYearidValueNavigation { get; set; }
        public virtual ICollection<PtasFundfactordetail> PtasFundfactordetail { get; set; }
    }
}

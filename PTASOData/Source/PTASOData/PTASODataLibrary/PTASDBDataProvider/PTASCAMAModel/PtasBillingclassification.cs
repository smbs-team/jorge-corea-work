using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasBillingclassification
    {
        public PtasBillingclassification()
        {
            PtasBillingcode = new HashSet<PtasBillingcode>();
        }

        public Guid PtasBillingclassificationid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasBillingclassificationgroup { get; set; }
        public string PtasBillingtype { get; set; }
        public int? PtasDataorigin { get; set; }
        public string PtasDescription { get; set; }
        public string PtasEcommercelabel { get; set; }
        public string PtasInterestcode { get; set; }
        public string PtasName { get; set; }
        public string PtasPenaltycode { get; set; }
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
        public Guid? PtasLevycodeidValue { get; set; }
        public Guid? PtasYearidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasLevycode PtasLevycodeidValueNavigation { get; set; }
        public virtual PtasYear PtasYearidValueNavigation { get; set; }
        public virtual ICollection<PtasBillingcode> PtasBillingcode { get; set; }
    }
}

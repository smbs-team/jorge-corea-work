using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasRefundpetitionlevyrate
    {
        public PtasRefundpetitionlevyrate()
        {
            PtasRefundpetition = new HashSet<PtasRefundpetition>();
        }

        public Guid PtasRefundpetitionlevyrateid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasLevycode { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasRegularrate { get; set; }
        public decimal? PtasSeniorrate { get; set; }
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
        public Guid? PtasYearidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasYear PtasYearidValueNavigation { get; set; }
        public virtual ICollection<PtasRefundpetition> PtasRefundpetition { get; set; }
    }
}

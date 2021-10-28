using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasExemption
    {
        public PtasExemption()
        {
            PtasSeapplication = new HashSet<PtasSeapplication>();
            PtasSeapplicationtask = new HashSet<PtasSeapplicationtask>();
        }

        public Guid PtasExemptionid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAccountnumber { get; set; }
        public DateTime? PtasEnddate { get; set; }
        public decimal? PtasExemptionamount { get; set; }
        public decimal? PtasExemptionamountBase { get; set; }
        public int? PtasExemptioncategory { get; set; }
        public string PtasName { get; set; }
        public DateTimeOffset? PtasRenewalcancellationnotificationsenton { get; set; }
        public DateTime? PtasRenewaldate { get; set; }
        public DateTime? PtasRenewalnotificationsenton { get; set; }
        public DateTimeOffset? PtasRenewalremindernotificationsenton { get; set; }
        public DateTime? PtasStartdate { get; set; }
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
        public Guid? PtasContactidValue { get; set; }
        public Guid? PtasExcemptionsidValue { get; set; }
        public Guid? PtasParceldetailidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual Contact PtasContactidValueNavigation { get; set; }
        public virtual PtasHomeimprovement PtasExcemptionsidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParceldetailidValueNavigation { get; set; }
        public virtual ICollection<PtasSeapplication> PtasSeapplication { get; set; }
        public virtual ICollection<PtasSeapplicationtask> PtasSeapplicationtask { get; set; }
    }
}

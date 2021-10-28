using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSeapplicationtask
    {
        public PtasSeapplicationtask()
        {
            PtasSeappnote = new HashSet<PtasSeappnote>();
        }

        public Guid PtasSeapplicationtaskid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? PtasAgeattimeofdeath { get; set; }
        public DateTime? PtasDateofdeath { get; set; }
        public bool? PtasDeathcertificate { get; set; }
        public string PtasDescription { get; set; }
        public DateTimeOffset? PtasDuedate { get; set; }
        public bool? PtasEmailsent { get; set; }
        public int? PtasEmailtosend { get; set; }
        public string PtasName { get; set; }
        public bool? PtasObituary { get; set; }
        public string PtasObituarycity { get; set; }
        public int? PtasObituarysource { get; set; }
        public string PtasObituarystate { get; set; }
        public DateTimeOffset? PtasRenewalcancellation { get; set; }
        public DateTimeOffset? PtasRenewalnotice1 { get; set; }
        public DateTimeOffset? PtasRenewalnotice2 { get; set; }
        public bool? PtasResolvetask { get; set; }
        public DateTimeOffset? PtasStartdate { get; set; }
        public DateTimeOffset? PtasSurvivingspouseinforeceivedon { get; set; }
        public DateTimeOffset? PtasSurvivingspouseinfosenton { get; set; }
        public int? PtasTasktype { get; set; }
        public bool? PtasVerified { get; set; }
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
        public Guid? PtasExemptionValue { get; set; }
        public Guid? PtasSaleValue { get; set; }
        public Guid? PtasSeapplicationidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual Contact PtasContactidValueNavigation { get; set; }
        public virtual PtasExemption PtasExemptionValueNavigation { get; set; }
        public virtual PtasSales PtasSaleValueNavigation { get; set; }
        public virtual PtasSeapplication PtasSeapplicationidValueNavigation { get; set; }
        public virtual ICollection<PtasSeappnote> PtasSeappnote { get; set; }
    }
}

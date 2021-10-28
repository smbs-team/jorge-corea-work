using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasFundfactordetail
    {
        public Guid PtasFundfactordetailid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasCostcenter { get; set; }
        public string PtasEbsfundnumber { get; set; }
        public decimal? PtasFactor { get; set; }
        public string PtasMainaccount { get; set; }
        public string PtasName { get; set; }
        public string PtasProject { get; set; }
        public decimal? PtasRate { get; set; }
        public int? PtasRatetype { get; set; }
        public string PtasRatetypetext { get; set; }
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
        public Guid? PtasBillingcodeidValue { get; set; }
        public Guid? PtasEbscostcenteridValue { get; set; }
        public Guid? PtasEbsfundnumberidValue { get; set; }
        public Guid? PtasEbsmainaccountidValue { get; set; }
        public Guid? PtasEbsprojectidValue { get; set; }
        public Guid? PtasFundallocationidValue { get; set; }
        public Guid? PtasFundidValue { get; set; }
        public Guid? PtasLevycodeidValue { get; set; }
        public Guid? PtasPtasFundfactordetailidValue { get; set; }
        public Guid? PtasYearidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasBillingcode PtasBillingcodeidValueNavigation { get; set; }
        public virtual PtasEbscostcenter PtasEbscostcenteridValueNavigation { get; set; }
        public virtual PtasEbsfundnumber PtasEbsfundnumberidValueNavigation { get; set; }
        public virtual PtasEbsmainaccount PtasEbsmainaccountidValueNavigation { get; set; }
        public virtual PtasEbsproject PtasEbsprojectidValueNavigation { get; set; }
        public virtual PtasFundallocation PtasFundallocationidValueNavigation { get; set; }
        public virtual PtasFund PtasFundidValueNavigation { get; set; }
        public virtual PtasLevycode PtasLevycodeidValueNavigation { get; set; }
        public virtual PtasRatesheetdetail PtasPtasFundfactordetailidValueNavigation { get; set; }
        public virtual PtasYear PtasYearidValueNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasFundallocation
    {
        public PtasFundallocation()
        {
            PtasFundfactordetail = new HashSet<PtasFundfactordetail>();
        }

        public Guid PtasFundallocationid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAllocation { get; set; }
        public string PtasCostcenter { get; set; }
        public string PtasEbsfundnumber { get; set; }
        public string PtasMainaccountnumber { get; set; }
        public string PtasName { get; set; }
        public string PtasProject { get; set; }
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
        public Guid? PtasEbscostcenteridValue { get; set; }
        public Guid? PtasEbsfundnumberidValue { get; set; }
        public Guid? PtasEbsmainaccountidValue { get; set; }
        public Guid? PtasEbsprojectidValue { get; set; }
        public Guid? PtasFundidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasEbscostcenter PtasEbscostcenteridValueNavigation { get; set; }
        public virtual PtasEbsfundnumber PtasEbsfundnumberidValueNavigation { get; set; }
        public virtual PtasEbsmainaccount PtasEbsmainaccountidValueNavigation { get; set; }
        public virtual PtasEbsproject PtasEbsprojectidValueNavigation { get; set; }
        public virtual PtasFund PtasFundidValueNavigation { get; set; }
        public virtual ICollection<PtasFundfactordetail> PtasFundfactordetail { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasFund
    {
        public PtasFund()
        {
            PtasBillingcode = new HashSet<PtasBillingcode>();
            PtasFundallocation = new HashSet<PtasFundallocation>();
            PtasFundfactordetail = new HashSet<PtasFundfactordetail>();
        }

        public Guid PtasFundid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? PtasAllocation { get; set; }
        public decimal? PtasAllocationtotal { get; set; }
        public DateTimeOffset? PtasAllocationtotalDate { get; set; }
        public int? PtasAllocationtotalState { get; set; }
        public string PtasCostcenter { get; set; }
        public string PtasDescription { get; set; }
        public string PtasEbsfundnumber { get; set; }
        public decimal? PtasFundallocationtotal { get; set; }
        public int? PtasLimit { get; set; }
        public string PtasMainaccount { get; set; }
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
        public Guid? PtasFundtypeidValue { get; set; }
        public Guid? PtasLevylidliftorbondValue { get; set; }
        public Guid? PtasTaxdistrictidValue { get; set; }

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
        public virtual PtasFundtype PtasFundtypeidValueNavigation { get; set; }
        public virtual PtasLevylidliftbond PtasLevylidliftorbondValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasTaxdistrictidValueNavigation { get; set; }
        public virtual ICollection<PtasBillingcode> PtasBillingcode { get; set; }
        public virtual ICollection<PtasFundallocation> PtasFundallocation { get; set; }
        public virtual ICollection<PtasFundfactordetail> PtasFundfactordetail { get; set; }
    }
}

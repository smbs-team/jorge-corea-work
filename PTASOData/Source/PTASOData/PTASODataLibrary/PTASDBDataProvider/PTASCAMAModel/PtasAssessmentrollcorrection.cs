using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAssessmentrollcorrection
    {
        public PtasAssessmentrollcorrection()
        {
            PtasFileattachmentmetadata = new HashSet<PtasFileattachmentmetadata>();
        }

        public Guid PtasAssessmentrollcorrectionid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public DateTimeOffset? PtasApprovaldate { get; set; }
        public bool? PtasApprovearc { get; set; }
        public decimal? PtasBilldifference { get; set; }
        public decimal? PtasBilldifferenceBase { get; set; }
        public DateTimeOffset? PtasDatesubmitted { get; set; }
        public decimal? PtasLevyrate { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasNewbill { get; set; }
        public decimal? PtasNewbillBase { get; set; }
        public decimal? PtasNewpenaltyamount { get; set; }
        public decimal? PtasNewpenaltyamountBase { get; set; }
        public int? PtasNewpenaltypercent { get; set; }
        public decimal? PtasNewvalue { get; set; }
        public decimal? PtasNewvalueBase { get; set; }
        public bool? PtasOktopost { get; set; }
        public decimal? PtasOldbill { get; set; }
        public decimal? PtasOldbillBase { get; set; }
        public decimal? PtasOldpenaltyamount { get; set; }
        public decimal? PtasOldpenaltyamountBase { get; set; }
        public int? PtasOldpenaltypercent { get; set; }
        public decimal? PtasOldvalue { get; set; }
        public decimal? PtasOldvalueBase { get; set; }
        public string PtasOrdernumber { get; set; }
        public decimal? PtasPaidamount { get; set; }
        public decimal? PtasPaidamountBase { get; set; }
        public DateTimeOffset? PtasPaiddate { get; set; }
        public string PtasPetitionnumber { get; set; }
        public DateTimeOffset? PtasPosteddate { get; set; }
        public decimal? PtasReceipt1 { get; set; }
        public decimal? PtasReceipt1Base { get; set; }
        public decimal? PtasReceipt2 { get; set; }
        public decimal? PtasReceipt2Base { get; set; }
        public string PtasReferencetransaction { get; set; }
        public bool? PtasRefundonly { get; set; }
        public DateTimeOffset? PtasSeconddate { get; set; }
        public bool? PtasSubmitforapproval { get; set; }
        public bool? PtasTrconly { get; set; }
        public bool? PtasTrcwithpreviousyearrefunds { get; set; }
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
        public Guid? PtasApprovalappraiseridValue { get; set; }
        public Guid? PtasAssessmenthistoryidValue { get; set; }
        public Guid? PtasAssessmentyearidValue { get; set; }
        public Guid? PtasLevycodeidValue { get; set; }
        public Guid? PtasPersonalpropertyaccountidValue { get; set; }
        public Guid? PtasPostedbyidValue { get; set; }
        public Guid? PtasReasoncodeidValue { get; set; }
        public Guid? PtasResponsibleappraiseridValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual Systemuser PtasApprovalappraiseridValueNavigation { get; set; }
        public virtual PtasPersonalpropertyhistory PtasAssessmenthistoryidValueNavigation { get; set; }
        public virtual PtasYear PtasAssessmentyearidValueNavigation { get; set; }
        public virtual PtasLevycode PtasLevycodeidValueNavigation { get; set; }
        public virtual PtasPersonalproperty PtasPersonalpropertyaccountidValueNavigation { get; set; }
        public virtual Systemuser PtasPostedbyidValueNavigation { get; set; }
        public virtual PtasArcreasoncode PtasReasoncodeidValueNavigation { get; set; }
        public virtual Systemuser PtasResponsibleappraiseridValueNavigation { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadata { get; set; }
    }
}

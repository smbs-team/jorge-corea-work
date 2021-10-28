using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasAbstractdocument
    {
        public Guid PtasAbstractdocumentid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public DateTimeOffset? PtasAssigneddate { get; set; }
        public decimal? PtasCheckamount { get; set; }
        public decimal? PtasCheckamountBase { get; set; }
        public string PtasChecknumber { get; set; }
        public string PtasCheckpayee { get; set; }
        public string PtasContactinfo { get; set; }
        public DateTimeOffset? PtasDatemoreinforequested { get; set; }
        public DateTimeOffset? PtasDeliveredrecordersoffice { get; set; }
        public bool? PtasDocneedsrecording { get; set; }
        public int? PtasDocumentformat { get; set; }
        public string PtasDocumentformatother { get; set; }
        public string PtasExcisedocnumber { get; set; }
        public string PtasLinktodoclandmark { get; set; }
        public string PtasName { get; set; }
        public string PtasNotes { get; set; }
        public string PtasOwnername { get; set; }
        public int? PtasPaymenttype { get; set; }
        public DateTimeOffset? PtasReceiveddate { get; set; }
        public string PtasReceivedfrom { get; set; }
        public DateTimeOffset? PtasRecievedrecordersoffice { get; set; }
        public string PtasRecordingnumber { get; set; }
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
        public Guid? PtasAbstractprojectidValue { get; set; }
        public Guid? PtasParceldetailidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasAbstractproject PtasAbstractprojectidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParceldetailidValueNavigation { get; set; }
    }
}

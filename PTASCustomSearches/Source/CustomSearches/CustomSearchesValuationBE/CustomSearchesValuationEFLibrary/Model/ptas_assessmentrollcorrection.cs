using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_assessmentrollcorrection
    {
        public ptas_assessmentrollcorrection()
        {
            ptas_fileattachmentmetadata = new HashSet<ptas_fileattachmentmetadata>();
        }

        public Guid ptas_assessmentrollcorrectionid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public decimal? ptas_billdifference { get; set; }
        public decimal? ptas_billdifference_base { get; set; }
        public decimal? ptas_levyrate { get; set; }
        public string ptas_name { get; set; }
        public decimal? ptas_newbill { get; set; }
        public decimal? ptas_newbill_base { get; set; }
        public int? ptas_newpenaltypercent { get; set; }
        public decimal? ptas_newvalue { get; set; }
        public decimal? ptas_newvalue_base { get; set; }
        public decimal? ptas_oldbill { get; set; }
        public decimal? ptas_oldbill_base { get; set; }
        public int? ptas_oldpenaltypercent { get; set; }
        public decimal? ptas_oldvalue { get; set; }
        public decimal? ptas_oldvalue_base { get; set; }
        public decimal? ptas_paidamount { get; set; }
        public decimal? ptas_paidamount_base { get; set; }
        public DateTimeOffset? ptas_paiddate { get; set; }
        public decimal? ptas_receipt1 { get; set; }
        public decimal? ptas_receipt1_base { get; set; }
        public decimal? ptas_receipt2 { get; set; }
        public decimal? ptas_receipt2_base { get; set; }
        public string ptas_referencetransaction { get; set; }
        public bool? ptas_refundonly { get; set; }
        public DateTimeOffset? ptas_seconddate { get; set; }
        public bool? ptas_trconly { get; set; }
        public bool? ptas_trcwithpreviousyearrefunds { get; set; }
        public int? statecode { get; set; }
        public int? statuscode { get; set; }
        public int? timezoneruleversionnumber { get; set; }
        public int? utcconversiontimezonecode { get; set; }
        public long? versionnumber { get; set; }
        public Guid? _createdby_value { get; set; }
        public Guid? _createdonbehalfby_value { get; set; }
        public Guid? _modifiedby_value { get; set; }
        public Guid? _modifiedonbehalfby_value { get; set; }
        public Guid? _ownerid_value { get; set; }
        public Guid? _owningbusinessunit_value { get; set; }
        public Guid? _owningteam_value { get; set; }
        public Guid? _owninguser_value { get; set; }
        public Guid? _ptas_approvalappraiserid_value { get; set; }
        public Guid? _ptas_assessmentyearid_value { get; set; }
        public Guid? _ptas_levycodeid_value { get; set; }
        public Guid? _ptas_personalpropertyaccountid_value { get; set; }
        public Guid? _ptas_reasoncodeid_value { get; set; }
        public Guid? _ptas_responsibleappraiserid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual systemuser _ptas_approvalappraiserid_valueNavigation { get; set; }
        public virtual ptas_year _ptas_assessmentyearid_valueNavigation { get; set; }
        public virtual ptas_levycode _ptas_levycodeid_valueNavigation { get; set; }
        public virtual ptas_arcreasoncode _ptas_reasoncodeid_valueNavigation { get; set; }
        public virtual systemuser _ptas_responsibleappraiserid_valueNavigation { get; set; }
        public virtual ICollection<ptas_fileattachmentmetadata> ptas_fileattachmentmetadata { get; set; }
    }
}

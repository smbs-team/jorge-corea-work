using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_fileattachmentmetadata
    {
        public ptas_fileattachmentmetadata()
        {
            ptas_quickcollect = new HashSet<ptas_quickcollect>();
        }

        public Guid ptas_fileattachmentmetadataid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public string ptas_accountnumber { get; set; }
        public string ptas_bloburl { get; set; }
        public bool? ptas_createseapp { get; set; }
        public string ptas_description { get; set; }
        public DateTimeOffset? ptas_documentdate { get; set; }
        public string ptas_documentlink { get; set; }
        public int? ptas_documentsize { get; set; }
        public string ptas_documenttype { get; set; }
        public string ptas_fileextension { get; set; }
        public int? ptas_filelibrary { get; set; }
        public int? ptas_filesource { get; set; }
        public int? ptas_filingmethod { get; set; }
        public string ptas_icscheckedoutbyusername { get; set; }
        public DateTimeOffset? ptas_icscheckedoutdate { get; set; }
        public string ptas_icscreatedbyusername { get; set; }
        public string ptas_icsdocumentid { get; set; }
        public DateTimeOffset? ptas_icsentereddate { get; set; }
        public string ptas_icsfileid { get; set; }
        public string ptas_icsfullindex { get; set; }
        public int? ptas_icsisinworkflow { get; set; }
        public DateTimeOffset? ptas_icsmodifieddate { get; set; }
        public DateTimeOffset? ptas_insertdate { get; set; }
        public bool? ptas_isblob { get; set; }
        public bool? ptas_isilinx { get; set; }
        public bool? ptas_issharepoint { get; set; }
        public int? ptas_listingstatus { get; set; }
        public DateTimeOffset? ptas_loaddate { get; set; }
        public string ptas_loginuserid { get; set; }
        public string ptas_name { get; set; }
        public string ptas_originalfilename { get; set; }
        public int? ptas_pagecount { get; set; }
        public int? ptas_paymentperiod { get; set; }
        public string ptas_portaldocument { get; set; }
        public string ptas_portalsection { get; set; }
        public int? ptas_ppdocumenttype { get; set; }
        public int? ptas_recid { get; set; }
        public string ptas_redactionurl { get; set; }
        public string ptas_repositoryname { get; set; }
        public bool? ptas_revisedlisting { get; set; }
        public int? ptas_rollyear { get; set; }
        public DateTimeOffset? ptas_scandate { get; set; }
        public DateTimeOffset? ptas_scandatetime { get; set; }
        public string ptas_scannerid { get; set; }
        public string ptas_sharepointurl { get; set; }
        public DateTimeOffset? ptas_updatedate { get; set; }
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
        public Guid? _ptas_arcid_value { get; set; }
        public Guid? _ptas_attachmentid_value { get; set; }
        public Guid? _ptas_loadbyid_value { get; set; }
        public Guid? _ptas_parcelid_value { get; set; }
        public Guid? _ptas_personalpropertyaccountid_value { get; set; }
        public Guid? _ptas_rollyearid_value { get; set; }
        public Guid? _ptas_seniorexemptionapplication_value { get; set; }
        public Guid? _ptas_seniorexemptionapplicationdetail_value { get; set; }
        public Guid? _ptas_taxaccountid_value { get; set; }
        public Guid? _ptas_taxpayerid_value { get; set; }
        public Guid? _ptas_taxrollcorrectionid_value { get; set; }
        public Guid? _ptas_taxyearid_value { get; set; }
        public Guid? _ptas_yearid_value { get; set; }

        public virtual systemuser _createdby_valueNavigation { get; set; }
        public virtual systemuser _createdonbehalfby_valueNavigation { get; set; }
        public virtual systemuser _modifiedby_valueNavigation { get; set; }
        public virtual systemuser _modifiedonbehalfby_valueNavigation { get; set; }
        public virtual team _owningteam_valueNavigation { get; set; }
        public virtual systemuser _owninguser_valueNavigation { get; set; }
        public virtual ptas_assessmentrollcorrection _ptas_arcid_valueNavigation { get; set; }
        public virtual ptas_homeimprovement _ptas_attachmentid_valueNavigation { get; set; }
        public virtual systemuser _ptas_loadbyid_valueNavigation { get; set; }
        public virtual ptas_parceldetail _ptas_parcelid_valueNavigation { get; set; }
        public virtual ptas_year _ptas_rollyearid_valueNavigation { get; set; }
        public virtual ptas_taxaccount _ptas_taxaccountid_valueNavigation { get; set; }
        public virtual ptas_taxaccount _ptas_taxpayerid_valueNavigation { get; set; }
        public virtual ptas_year _ptas_taxyearid_valueNavigation { get; set; }
        public virtual ptas_year _ptas_yearid_valueNavigation { get; set; }
        public virtual ICollection<ptas_quickcollect> ptas_quickcollect { get; set; }
    }
}

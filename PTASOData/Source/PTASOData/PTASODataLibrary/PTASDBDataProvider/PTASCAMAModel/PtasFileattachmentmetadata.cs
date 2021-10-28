using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasFileattachmentmetadata
    {
        public PtasFileattachmentmetadata()
        {
            PtasQuickcollect = new HashSet<PtasQuickcollect>();
        }

        public Guid PtasFileattachmentmetadataid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAccountnumber { get; set; }
        public string PtasBloburl { get; set; }
        public bool? PtasCreateseapp { get; set; }
        public string PtasDescription { get; set; }
        public DateTimeOffset? PtasDocumentdate { get; set; }
        public string PtasDocumentlink { get; set; }
        public int? PtasDocumentsize { get; set; }
        public string PtasDocumenttype { get; set; }
        public string PtasFileextension { get; set; }
        public int? PtasFilelibrary { get; set; }
        public int? PtasFilesource { get; set; }
        public int? PtasFilingmethod { get; set; }
        public string PtasIcscheckedoutbyusername { get; set; }
        public DateTimeOffset? PtasIcscheckedoutdate { get; set; }
        public string PtasIcscreatedbyusername { get; set; }
        public string PtasIcsdocumentid { get; set; }
        public DateTimeOffset? PtasIcsentereddate { get; set; }
        public string PtasIcsfileid { get; set; }
        public string PtasIcsfullindex { get; set; }
        public int? PtasIcsisinworkflow { get; set; }
        public DateTimeOffset? PtasIcsmodifieddate { get; set; }
        public DateTimeOffset? PtasInsertdate { get; set; }
        public bool? PtasIsblob { get; set; }
        public bool? PtasIsilinx { get; set; }
        public bool? PtasIssharepoint { get; set; }
        public int? PtasListingstatus { get; set; }
        public DateTimeOffset? PtasLoaddate { get; set; }
        public string PtasLoginuserid { get; set; }
        public string PtasName { get; set; }
        public string PtasOriginalfilename { get; set; }
        public int? PtasPagecount { get; set; }
        public int? PtasPaymentperiod { get; set; }
        public string PtasPortaldocument { get; set; }
        public string PtasPortalsection { get; set; }
        public int? PtasPpdocumenttype { get; set; }
        public int? PtasRecid { get; set; }
        public string PtasRedactionurl { get; set; }
        public string PtasRepositoryname { get; set; }
        public bool? PtasRevisedlisting { get; set; }
        public int? PtasRollyear { get; set; }
        public DateTimeOffset? PtasScandate { get; set; }
        public DateTimeOffset? PtasScandatetime { get; set; }
        public string PtasScannerid { get; set; }
        public string PtasSharepointurl { get; set; }
        public DateTimeOffset? PtasUpdatedate { get; set; }
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
        public Guid? PtasArcidValue { get; set; }
        public Guid? PtasAttachmentidValue { get; set; }
        public Guid? PtasLoadbyidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasPersonalpropertyaccountidValue { get; set; }
        public Guid? PtasRollyearidValue { get; set; }
        public Guid? PtasSeniorexemptionapplicationValue { get; set; }
        public Guid? PtasSeniorexemptionapplicationdetailValue { get; set; }
        public Guid? PtasTaxaccountidValue { get; set; }
        public Guid? PtasTaxpayeridValue { get; set; }
        public Guid? PtasTaxrollcorrectionidValue { get; set; }
        public Guid? PtasTaxyearidValue { get; set; }
        public Guid? PtasYearidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasAssessmentrollcorrection PtasArcidValueNavigation { get; set; }
        public virtual PtasHomeimprovement PtasAttachmentidValueNavigation { get; set; }
        public virtual Systemuser PtasLoadbyidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasPersonalproperty PtasPersonalpropertyaccountidValueNavigation { get; set; }
        public virtual PtasYear PtasRollyearidValueNavigation { get; set; }
        public virtual PtasSeapplication PtasSeniorexemptionapplicationValueNavigation { get; set; }
        public virtual PtasSeappdetail PtasSeniorexemptionapplicationdetailValueNavigation { get; set; }
        public virtual PtasTaxaccount PtasTaxaccountidValueNavigation { get; set; }
        public virtual PtasTaxaccount PtasTaxpayeridValueNavigation { get; set; }
        public virtual PtasTaxrollcorrection PtasTaxrollcorrectionidValueNavigation { get; set; }
        public virtual PtasYear PtasTaxyearidValueNavigation { get; set; }
        public virtual PtasYear PtasYearidValueNavigation { get; set; }
        public virtual ICollection<PtasQuickcollect> PtasQuickcollect { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasLevycode
    {
        public PtasLevycode()
        {
            PtasAbstractproject = new HashSet<PtasAbstractproject>();
            PtasAbstractprojectresultparcel = new HashSet<PtasAbstractprojectresultparcel>();
            PtasAnnexationparcelreview = new HashSet<PtasAnnexationparcelreview>();
            PtasAptadjustedlevyrate = new HashSet<PtasAptadjustedlevyrate>();
            PtasAptvaluation = new HashSet<PtasAptvaluation>();
            PtasAssessmentrollcorrection = new HashSet<PtasAssessmentrollcorrection>();
            PtasBillingclassification = new HashSet<PtasBillingclassification>();
            PtasBillingcode = new HashSet<PtasBillingcode>();
            PtasCurrentusevaluehistory = new HashSet<PtasCurrentusevaluehistory>();
            PtasFundfactordetail = new HashSet<PtasFundfactordetail>();
            PtasLevycodechangePtasChangefromlevycodeidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasLevycodechangePtasChangetolevycodeidValueNavigation = new HashSet<PtasLevycodechange>();
            PtasOmitPtasLevycodeidValueNavigation = new HashSet<PtasOmit>();
            PtasOmitPtasOmitlevycodeidValueNavigation = new HashSet<PtasOmit>();
            PtasParceldetail = new HashSet<PtasParceldetail>();
            PtasPersonalproperty = new HashSet<PtasPersonalproperty>();
            PtasPersonalpropertyhistory = new HashSet<PtasPersonalpropertyhistory>();
            PtasTaxaccount = new HashSet<PtasTaxaccount>();
            PtasTaxdistrict = new HashSet<PtasTaxdistrict>();
            PtasTaxrollcorrection = new HashSet<PtasTaxrollcorrection>();
            PtasTaxrollcorrectionvalue = new HashSet<PtasTaxrollcorrectionvalue>();
        }

        public Guid PtasLevycodeid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public decimal? Ptas9998590checktotal { get; set; }
        public decimal? PtasCurrent1constitutionallimit { get; set; }
        public string PtasDescription { get; set; }
        public int? PtasLevydodetype { get; set; }
        public decimal? PtasLocallevytotallimit { get; set; }
        public string PtasName { get; set; }
        public decimal? PtasOverunderconstitutional { get; set; }
        public decimal? PtasTotallevyrateconstitutional { get; set; }
        public int? Statecode { get; set; }
        public int? Statuscode { get; set; }
        public int? Timezoneruleversionnumber { get; set; }
        public int? Utcconversiontimezonecode { get; set; }
        public long? Versionnumber { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? OrganizationidValue { get; set; }
        public Guid? PtasFiredistrictidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual PtasTaxdistrict PtasFiredistrictidValueNavigation { get; set; }
        public virtual ICollection<PtasAbstractproject> PtasAbstractproject { get; set; }
        public virtual ICollection<PtasAbstractprojectresultparcel> PtasAbstractprojectresultparcel { get; set; }
        public virtual ICollection<PtasAnnexationparcelreview> PtasAnnexationparcelreview { get; set; }
        public virtual ICollection<PtasAptadjustedlevyrate> PtasAptadjustedlevyrate { get; set; }
        public virtual ICollection<PtasAptvaluation> PtasAptvaluation { get; set; }
        public virtual ICollection<PtasAssessmentrollcorrection> PtasAssessmentrollcorrection { get; set; }
        public virtual ICollection<PtasBillingclassification> PtasBillingclassification { get; set; }
        public virtual ICollection<PtasBillingcode> PtasBillingcode { get; set; }
        public virtual ICollection<PtasCurrentusevaluehistory> PtasCurrentusevaluehistory { get; set; }
        public virtual ICollection<PtasFundfactordetail> PtasFundfactordetail { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasChangefromlevycodeidValueNavigation { get; set; }
        public virtual ICollection<PtasLevycodechange> PtasLevycodechangePtasChangetolevycodeidValueNavigation { get; set; }
        public virtual ICollection<PtasOmit> PtasOmitPtasLevycodeidValueNavigation { get; set; }
        public virtual ICollection<PtasOmit> PtasOmitPtasOmitlevycodeidValueNavigation { get; set; }
        public virtual ICollection<PtasParceldetail> PtasParceldetail { get; set; }
        public virtual ICollection<PtasPersonalproperty> PtasPersonalproperty { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistory { get; set; }
        public virtual ICollection<PtasTaxaccount> PtasTaxaccount { get; set; }
        public virtual ICollection<PtasTaxdistrict> PtasTaxdistrict { get; set; }
        public virtual ICollection<PtasTaxrollcorrection> PtasTaxrollcorrection { get; set; }
        public virtual ICollection<PtasTaxrollcorrectionvalue> PtasTaxrollcorrectionvalue { get; set; }
    }
}

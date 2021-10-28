using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasQuickcollect
    {
        public PtasQuickcollect()
        {
            PtasPersonalpropertyhistory = new HashSet<PtasPersonalpropertyhistory>();
        }

        public Guid PtasQuickcollectid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAccountnumber { get; set; }
        public decimal? PtasAssessmentvalue { get; set; }
        public decimal? PtasAssessmentvalueBase { get; set; }
        public string PtasAttention { get; set; }
        public string PtasBusinessname { get; set; }
        public DateTimeOffset? PtasClosingdate { get; set; }
        public DateTimeOffset? PtasDate { get; set; }
        public int? PtasDispositionofassets { get; set; }
        public string PtasEmail { get; set; }
        public decimal? PtasEquipment { get; set; }
        public decimal? PtasEquipmentBase { get; set; }
        public string PtasFuturestatus { get; set; }
        public decimal? PtasIntangibles { get; set; }
        public decimal? PtasIntangiblesBase { get; set; }
        public decimal? PtasLeaseholdimprovements { get; set; }
        public decimal? PtasLeaseholdimprovementsBase { get; set; }
        public string PtasLegalentity { get; set; }
        public int? PtasMethodoftransfer { get; set; }
        public string PtasName { get; set; }
        public string PtasNewbusinessname { get; set; }
        public string PtasNewinformationAddrCity { get; set; }
        public string PtasNewinformationAddrLocationcity { get; set; }
        public string PtasNewinformationAddrLocationstreet1 { get; set; }
        public string PtasNewinformationAddrLocationstreet2 { get; set; }
        public string PtasNewinformationAddrLocationzip { get; set; }
        public string PtasNewinformationAddrStreet1 { get; set; }
        public string PtasNewinformationAddrStreet2 { get; set; }
        public string PtasNewinformationAddrZip { get; set; }
        public string PtasNewowneremail { get; set; }
        public string PtasNewownername { get; set; }
        public bool? PtasOktopost { get; set; }
        public decimal? PtasOther { get; set; }
        public decimal? PtasOtherBase { get; set; }
        public bool? PtasOutofcounty { get; set; }
        public string PtasOwnername { get; set; }
        public bool? PtasPaid { get; set; }
        public string PtasPersonalpropinfoAddrCity { get; set; }
        public string PtasPersonalpropinfoAddrLocationcity { get; set; }
        public string PtasPersonalpropinfoAddrLocationstreet1 { get; set; }
        public string PtasPersonalpropinfoAddrLocationstreet2 { get; set; }
        public string PtasPersonalpropinfoAddrLocationzip { get; set; }
        public string PtasPersonalpropinfoAddrStreet1 { get; set; }
        public string PtasPersonalpropinfoAddrStreet2 { get; set; }
        public string PtasPersonalpropinfoAddrZip { get; set; }
        public string PtasQuickcollectnumber { get; set; }
        public int? PtasReasonforrequest { get; set; }
        public string PtasRequestorinfoAddrCity { get; set; }
        public string PtasRequestorinfoAddrStreet1 { get; set; }
        public string PtasRequestorinfoAddrStreet2 { get; set; }
        public string PtasRequestorinfoAddrTelephone { get; set; }
        public string PtasRequestorinfoAddrZip { get; set; }
        public string PtasRequestorinfoBusinessname { get; set; }
        public string PtasTelephone { get; set; }
        public decimal? PtasTotalsalesprice { get; set; }
        public decimal? PtasTotalsalespriceBase { get; set; }
        public string PtasUbinumber { get; set; }
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
        public Guid? PtasBillofsaleFileattachementmetadataidValue { get; set; }
        public Guid? PtasNewinformationAddrLocationstateidValue { get; set; }
        public Guid? PtasNewinformationAddrStateidValue { get; set; }
        public Guid? PtasPersonalpropertyidValue { get; set; }
        public Guid? PtasPersonalpropinfoAddrLocationstateidValue { get; set; }
        public Guid? PtasPersonalpropinfoAddrStateidValue { get; set; }
        public Guid? PtasPpassessmenthistoryidValue { get; set; }
        public Guid? PtasProcesseduseridValue { get; set; }
        public Guid? PtasQuickcollectPersonalpropertyidValue { get; set; }
        public Guid? PtasRequestorinfoAddrStateidValue { get; set; }
        public Guid? PtasYearidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasFileattachmentmetadata PtasBillofsaleFileattachementmetadataidValueNavigation { get; set; }
        public virtual PtasStateorprovince PtasNewinformationAddrLocationstateidValueNavigation { get; set; }
        public virtual PtasStateorprovince PtasNewinformationAddrStateidValueNavigation { get; set; }
        public virtual PtasPersonalproperty PtasPersonalpropertyidValueNavigation { get; set; }
        public virtual PtasStateorprovince PtasPersonalpropinfoAddrLocationstateidValueNavigation { get; set; }
        public virtual PtasStateorprovince PtasPersonalpropinfoAddrStateidValueNavigation { get; set; }
        public virtual PtasPersonalpropertyhistory PtasPpassessmenthistoryidValueNavigation { get; set; }
        public virtual Systemuser PtasProcesseduseridValueNavigation { get; set; }
        public virtual PtasPersonalproperty PtasQuickcollectPersonalpropertyidValueNavigation { get; set; }
        public virtual PtasStateorprovince PtasRequestorinfoAddrStateidValueNavigation { get; set; }
        public virtual PtasYear PtasYearidValueNavigation { get; set; }
        public virtual ICollection<PtasPersonalpropertyhistory> PtasPersonalpropertyhistory { get; set; }
    }
}

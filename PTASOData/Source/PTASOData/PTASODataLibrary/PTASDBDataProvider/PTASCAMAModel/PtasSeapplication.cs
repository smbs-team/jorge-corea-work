﻿using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class PtasSeapplication
    {
        public PtasSeapplication()
        {
            PtasFileattachmentmetadata = new HashSet<PtasFileattachmentmetadata>();
            PtasRefundpetition = new HashSet<PtasRefundpetition>();
            PtasSeappdetail = new HashSet<PtasSeappdetail>();
            PtasSeapplicationtask = new HashSet<PtasSeapplicationtask>();
            PtasSeappnote = new HashSet<PtasSeappnote>();
            PtasSeappoccupant = new HashSet<PtasSeappoccupant>();
            PtasSeappotherprop = new HashSet<PtasSeappotherprop>();
            PtasSefrozenvalue = new HashSet<PtasSefrozenvalue>();
        }

        public Guid PtasSeapplicationid { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Exchangerate { get; set; }
        public int? Importsequencenumber { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string PtasAccountnumber { get; set; }
        public int? PtasAccounttype { get; set; }
        public bool? PtasAddrchange { get; set; }
        public string PtasAddrcity { get; set; }
        public string PtasAddrpostal { get; set; }
        public string PtasAddrstate { get; set; }
        public string PtasAddrstreet1 { get; set; }
        public int? PtasAgeattimeofdeath { get; set; }
        public int? PtasAlternatekey { get; set; }
        public DateTime? PtasApplicantdateofbirth { get; set; }
        public string PtasApplicantemailaddress { get; set; }
        public string PtasApplicantfirstname { get; set; }
        public string PtasApplicantlastname { get; set; }
        public string PtasApplicantmiddlename { get; set; }
        public string PtasApplicantmobilephone { get; set; }
        public string PtasApplicantphonenumber { get; set; }
        public string PtasApplicantsuffix { get; set; }
        public DateTimeOffset? PtasApplicationdate { get; set; }
        public DateTimeOffset? PtasApplicationstatuslastchangedon { get; set; }
        public int? PtasApplicationtype { get; set; }
        public bool? PtasApprovaltaskcreated { get; set; }
        public bool? PtasAutogeneratedocuments { get; set; }
        public bool? PtasBlockautoatc240 { get; set; }
        public string PtasCheckaddresscity { get; set; }
        public string PtasCheckaddressname { get; set; }
        public string PtasCheckaddresspostalcode { get; set; }
        public string PtasCheckaddressstate { get; set; }
        public string PtasCheckaddressstreet { get; set; }
        public string PtasCoopname { get; set; }
        public string PtasCoopownedshares { get; set; }
        public bool? PtasCoopproperty { get; set; }
        public string PtasCooptotalshares { get; set; }
        public string PtasCooptreasurer { get; set; }
        public string PtasCooptreasurerphone { get; set; }
        public string PtasCorrespondencename { get; set; }
        public decimal? PtasCurrentimprovementsvalue { get; set; }
        public decimal? PtasCurrentimprovementsvalueBase { get; set; }
        public decimal? PtasCurrentlandvalue { get; set; }
        public decimal? PtasCurrentlandvalueBase { get; set; }
        public bool? PtasCurrentlyownoccupy { get; set; }
        public DateTime? PtasDateofdeath { get; set; }
        public DateTime? PtasDateofformerpropertysale { get; set; }
        public DateTime? PtasDatepropertypurchased { get; set; }
        public DateTime? PtasDatepropertysoldin2020 { get; set; }
        public DateTimeOffset? PtasDatereceived { get; set; }
        public int? PtasDayslaststatuschange { get; set; }
        public bool? PtasDeceasedspouseex { get; set; }
        public bool? PtasDenialtaskcreated { get; set; }
        public bool? PtasDifferentcheckaddress { get; set; }
        public bool? PtasDisabled { get; set; }
        public bool? PtasDivorcedlegallyseparated { get; set; }
        public bool? PtasDocageapplicant { get; set; }
        public bool? PtasDocapplicant { get; set; }
        public bool? PtasDocbirthcertificate { get; set; }
        public bool? PtasDoccoopshares { get; set; }
        public bool? PtasDoccotenant { get; set; }
        public bool? PtasDocdisability { get; set; }
        public bool? PtasDocdriverslicense { get; set; }
        public bool? PtasDocmajorlifechange { get; set; }
        public string PtasDocother { get; set; }
        public bool? PtasDocownership { get; set; }
        public bool? PtasDocpassport { get; set; }
        public bool? PtasDocspouse { get; set; }
        public bool? PtasDocspouseage { get; set; }
        public DateTime? PtasEffectivedateofdisability { get; set; }
        public int? PtasExemptionlevel { get; set; }
        public string PtasExemptionlevelyear { get; set; }
        public bool? PtasFinancialsection { get; set; }
        public DateTimeOffset? PtasFirstdateprimaryres { get; set; }
        public decimal? PtasFrozenimprovementvalue { get; set; }
        public decimal? PtasFrozenimprovementvalueBase { get; set; }
        public decimal? PtasFrozenlandvalue { get; set; }
        public decimal? PtasFrozenlandvalueBase { get; set; }
        public bool? PtasHadexinanothercounty { get; set; }
        public bool? PtasHasspouseorpartner { get; set; }
        public bool? PtasHousingcoop { get; set; }
        public bool? PtasIsasurvivingspouse { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public bool? PtasLifeestate { get; set; }
        public bool? PtasMajorlifechange { get; set; }
        public bool? PtasMarried { get; set; }
        public bool? PtasMarriedlivingapart { get; set; }
        public string PtasMissingdocumentlist { get; set; }
        public bool? PtasMobilehome { get; set; }
        public string PtasMobilehomemake { get; set; }
        public string PtasMobilehomemodel { get; set; }
        public string PtasMobilehomeyear { get; set; }
        public string PtasName { get; set; }
        public bool? PtasObituary { get; set; }
        public string PtasObituarycity { get; set; }
        public int? PtasObituarysource { get; set; }
        public string PtasObituarystate { get; set; }
        public DateTimeOffset? PtasOccupieddate { get; set; }
        public string PtasOthercountyaddress { get; set; }
        public string PtasOthercountycity { get; set; }
        public string PtasOthercountypostal { get; set; }
        public string PtasOthercountystate { get; set; }
        public bool? PtasOtheroccupants { get; set; }
        public string PtasOtherowners { get; set; }
        public string PtasOtherparcelnumber { get; set; }
        public bool? PtasOwnmultipleproperties { get; set; }
        public bool? PtasOwnthroughtrust { get; set; }
        public decimal? PtasPercentagerentedout { get; set; }
        public decimal? PtasPercentageusedforbusiness { get; set; }
        public int? PtasProofofagedocuments { get; set; }
        public int? PtasProofofagedocumentsspouse { get; set; }
        public int? PtasProofofcoopsharesdocuments { get; set; }
        public int? PtasProofofdisabilitydocuments { get; set; }
        public int? PtasProofofmajorlifechangedocuments { get; set; }
        public int? PtasProofofownershipdocuments { get; set; }
        public bool? PtasPropertysection { get; set; }
        public bool? PtasPropertytaxesdelinquent { get; set; }
        public bool? PtasPropertyusedforbusiness { get; set; }
        public bool? PtasReceivedexemptionbefore { get; set; }
        public bool? PtasRentoutaportionofproperty { get; set; }
        public bool? PtasRequestouttotaxpayer { get; set; }
        public bool? PtasRequiredtofilefederalincometaxreturn { get; set; }
        public bool? PtasResidingfor9months { get; set; }
        public bool? PtasSendreminderadditionalinformation { get; set; }
        public bool? PtasSignatureconfirmed { get; set; }
        public DateTimeOffset? PtasSignaturedate { get; set; }
        public string PtasSignatureline { get; set; }
        public bool? PtasSignaturesection { get; set; }
        public bool? PtasSingle { get; set; }
        public bool? PtasSinglefamilyresidence { get; set; }
        public bool? PtasSingleunitofmultidwellingcondoorduplex { get; set; }
        public bool? PtasSoldformerresidence { get; set; }
        public bool? PtasSoldotherpropertyin2020 { get; set; }
        public int? PtasSource { get; set; }
        public int? PtasSplitcode { get; set; }
        public DateTimeOffset? PtasSpousedob { get; set; }
        public DateTimeOffset? PtasSpousedod { get; set; }
        public string PtasSpousefirstname { get; set; }
        public string PtasSpouselastname { get; set; }
        public string PtasSpousemiddlename { get; set; }
        public string PtasSpousesuffix { get; set; }
        public int? PtasSquarefootagerentedout { get; set; }
        public int? PtasSquarefootageusedforbusiness { get; set; }
        public int? PtasStatuschangecat { get; set; }
        public DateTimeOffset? PtasStatuschangedate { get; set; }
        public bool? PtasSubmittalconfirmationsent { get; set; }
        public DateTime? PtasSurvivingspouseinforeceivedon { get; set; }
        public DateTime? PtasSurvivingspouseinfosenton { get; set; }
        public DateTimeOffset? PtasTaxpayerremindersenton { get; set; }
        public DateTimeOffset? PtasTaxpayerrequestsenton { get; set; }
        public bool? PtasTaxpayersection { get; set; }
        public int? PtasTaxrefundrecipient { get; set; }
        public decimal? PtasTotalcurrentvalue { get; set; }
        public decimal? PtasTotalcurrentvalueBase { get; set; }
        public decimal? PtasTotalfrozenvalue { get; set; }
        public decimal? PtasTotalfrozenvalueBase { get; set; }
        public bool? PtasUnder61withdisabilitynotice { get; set; }
        public bool? PtasVadisabled { get; set; }
        public bool? PtasVeteran { get; set; }
        public bool? PtasVeteranwithserviceevaluationordisability { get; set; }
        public DateTime? PtasWhenwasthepreviousexemption { get; set; }
        public string PtasWherepropertywassoldin2020 { get; set; }
        public string PtasWherewasthepreviousexemption { get; set; }
        public bool? PtasWidowed { get; set; }
        public string PtasYearsappliedfor { get; set; }
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
        public Guid? PtasAddrcountryidValue { get; set; }
        public Guid? PtasApplicantrecordValue { get; set; }
        public Guid? PtasCheckaddresscountryidValue { get; set; }
        public Guid? PtasCompletedbyidValue { get; set; }
        public Guid? PtasContactidValue { get; set; }
        public Guid? PtasDecisionreasonidValue { get; set; }
        public Guid? PtasExemptionidValue { get; set; }
        public Guid? PtasFrozenyearidValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? PtasTaxyearidValue { get; set; }
        public Guid? PtasTransferredfrcountyValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual PtasCountry PtasAddrcountryidValueNavigation { get; set; }
        public virtual PtasSeappoccupant PtasApplicantrecordValueNavigation { get; set; }
        public virtual PtasCountry PtasCheckaddresscountryidValueNavigation { get; set; }
        public virtual Systemuser PtasCompletedbyidValueNavigation { get; set; }
        public virtual Contact PtasContactidValueNavigation { get; set; }
        public virtual PtasSeexemptionreason PtasDecisionreasonidValueNavigation { get; set; }
        public virtual PtasExemption PtasExemptionidValueNavigation { get; set; }
        public virtual PtasYear PtasFrozenyearidValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual PtasYear PtasTaxyearidValueNavigation { get; set; }
        public virtual PtasCounty PtasTransferredfrcountyValueNavigation { get; set; }
        public virtual ICollection<PtasFileattachmentmetadata> PtasFileattachmentmetadata { get; set; }
        public virtual ICollection<PtasRefundpetition> PtasRefundpetition { get; set; }
        public virtual ICollection<PtasSeappdetail> PtasSeappdetail { get; set; }
        public virtual ICollection<PtasSeapplicationtask> PtasSeapplicationtask { get; set; }
        public virtual ICollection<PtasSeappnote> PtasSeappnote { get; set; }
        public virtual ICollection<PtasSeappoccupant> PtasSeappoccupant { get; set; }
        public virtual ICollection<PtasSeappotherprop> PtasSeappotherprop { get; set; }
        public virtual ICollection<PtasSefrozenvalue> PtasSefrozenvalue { get; set; }
    }
}

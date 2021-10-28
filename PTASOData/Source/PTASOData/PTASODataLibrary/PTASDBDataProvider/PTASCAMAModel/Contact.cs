using System;
using System.Collections.Generic;

namespace PTASODataLibrary.PtasDbDataProvider.PtasCamaModel
{
    public partial class Contact
    {
        public Contact()
        {
            InverseMasteridValueNavigation = new HashSet<Contact>();
            InverseParentcustomeridValueNavigation = new HashSet<Contact>();
            PtasCurrentuseapplication = new HashSet<PtasCurrentuseapplication>();
            PtasExemption = new HashSet<PtasExemption>();
            PtasSeappdetail = new HashSet<PtasSeappdetail>();
            PtasSeapplication = new HashSet<PtasSeapplication>();
            PtasSeapplicationtask = new HashSet<PtasSeapplicationtask>();
            PtasSeappnote = new HashSet<PtasSeappnote>();
            PtasSefrozenvalue = new HashSet<PtasSefrozenvalue>();
        }

        public Guid Contactid { get; set; }
        public int? Accountrolecode { get; set; }
        public Guid? Address1Addressid { get; set; }
        public int? Address1Addresstypecode { get; set; }
        public string Address1City { get; set; }
        public string Address1Composite { get; set; }
        public string Address1Country { get; set; }
        public string Address1County { get; set; }
        public string Address1Fax { get; set; }
        public int? Address1Freighttermscode { get; set; }
        public double? Address1Latitude { get; set; }
        public string Address1Line1 { get; set; }
        public string Address1Line2 { get; set; }
        public string Address1Line3 { get; set; }
        public double? Address1Longitude { get; set; }
        public string Address1Name { get; set; }
        public string Address1Postalcode { get; set; }
        public string Address1Postofficebox { get; set; }
        public string Address1Primarycontactname { get; set; }
        public int? Address1Shippingmethodcode { get; set; }
        public string Address1Stateorprovince { get; set; }
        public string Address1Telephone1 { get; set; }
        public string Address1Telephone2 { get; set; }
        public string Address1Telephone3 { get; set; }
        public string Address1Upszone { get; set; }
        public int? Address1Utcoffset { get; set; }
        public Guid? Address2Addressid { get; set; }
        public int? Address2Addresstypecode { get; set; }
        public string Address2City { get; set; }
        public string Address2Composite { get; set; }
        public string Address2Country { get; set; }
        public string Address2County { get; set; }
        public string Address2Fax { get; set; }
        public int? Address2Freighttermscode { get; set; }
        public double? Address2Latitude { get; set; }
        public string Address2Line1 { get; set; }
        public string Address2Line2 { get; set; }
        public string Address2Line3 { get; set; }
        public double? Address2Longitude { get; set; }
        public string Address2Name { get; set; }
        public string Address2Postalcode { get; set; }
        public string Address2Postofficebox { get; set; }
        public string Address2Primarycontactname { get; set; }
        public int? Address2Shippingmethodcode { get; set; }
        public string Address2Stateorprovince { get; set; }
        public string Address2Telephone1 { get; set; }
        public string Address2Telephone2 { get; set; }
        public string Address2Telephone3 { get; set; }
        public string Address2Upszone { get; set; }
        public int? Address2Utcoffset { get; set; }
        public Guid? Address3Addressid { get; set; }
        public int? Address3Addresstypecode { get; set; }
        public string Address3City { get; set; }
        public string Address3Composite { get; set; }
        public string Address3Country { get; set; }
        public string Address3County { get; set; }
        public string Address3Fax { get; set; }
        public int? Address3Freighttermscode { get; set; }
        public double? Address3Latitude { get; set; }
        public string Address3Line1 { get; set; }
        public string Address3Line2 { get; set; }
        public string Address3Line3 { get; set; }
        public double? Address3Longitude { get; set; }
        public string Address3Name { get; set; }
        public string Address3Postalcode { get; set; }
        public string Address3Postofficebox { get; set; }
        public string Address3Primarycontactname { get; set; }
        public int? Address3Shippingmethodcode { get; set; }
        public string Address3Stateorprovince { get; set; }
        public string Address3Telephone1 { get; set; }
        public string Address3Telephone2 { get; set; }
        public string Address3Telephone3 { get; set; }
        public string Address3Upszone { get; set; }
        public int? Address3Utcoffset { get; set; }
        public decimal? Aging30 { get; set; }
        public decimal? Aging30Base { get; set; }
        public decimal? Aging60 { get; set; }
        public decimal? Aging60Base { get; set; }
        public decimal? Aging90 { get; set; }
        public decimal? Aging90Base { get; set; }
        public DateTime? Anniversary { get; set; }
        public decimal? Annualincome { get; set; }
        public decimal? AnnualincomeBase { get; set; }
        public string Assistantname { get; set; }
        public string Assistantphone { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Business2 { get; set; }
        public string Businesscard { get; set; }
        public string Businesscardattributes { get; set; }
        public string Callback { get; set; }
        public string Childrensnames { get; set; }
        public string Company { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public decimal? Creditlimit { get; set; }
        public decimal? CreditlimitBase { get; set; }
        public bool? Creditonhold { get; set; }
        public int? Customersizecode { get; set; }
        public int? Customertypecode { get; set; }
        public string Department { get; set; }
        public string Description { get; set; }
        public bool? Donotbulkemail { get; set; }
        public bool? Donotbulkpostalmail { get; set; }
        public bool? Donotemail { get; set; }
        public bool? Donotfax { get; set; }
        public bool? Donotphone { get; set; }
        public bool? Donotpostalmail { get; set; }
        public bool? Donotsendmm { get; set; }
        public int? Educationcode { get; set; }
        public string Emailaddress1 { get; set; }
        public string Emailaddress2 { get; set; }
        public string Emailaddress3 { get; set; }
        public string Employeeid { get; set; }
        public byte[] Entityimage { get; set; }
        public long? EntityimageTimestamp { get; set; }
        public string EntityimageUrl { get; set; }
        public Guid? Entityimageid { get; set; }
        public decimal? Exchangerate { get; set; }
        public string Externaluseridentifier { get; set; }
        public int? Familystatuscode { get; set; }
        public string Fax { get; set; }
        public string Firstname { get; set; }
        public bool? Followemail { get; set; }
        public string Ftpsiteurl { get; set; }
        public string Fullname { get; set; }
        public int? Gendercode { get; set; }
        public string Governmentid { get; set; }
        public int? Haschildrencode { get; set; }
        public string Home2 { get; set; }
        public int? Importsequencenumber { get; set; }
        public bool? Isbackofficecustomer { get; set; }
        public string Jobtitle { get; set; }
        public string Lastname { get; set; }
        public DateTimeOffset? Lastonholdtime { get; set; }
        public DateTimeOffset? Lastusedincampaign { get; set; }
        public int? Leadsourcecode { get; set; }
        public string Managername { get; set; }
        public string Managerphone { get; set; }
        public bool? Marketingonly { get; set; }
        public bool? Merged { get; set; }
        public string Middlename { get; set; }
        public string Mobilephone { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public bool? MsdynGdproptout { get; set; }
        public int? MsdynOrgchangestatus { get; set; }
        public string Nickname { get; set; }
        public int? Numberofchildren { get; set; }
        public int? Onholdtime { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public string Pager { get; set; }
        public bool? Participatesinworkflow { get; set; }
        public int? Paymenttermscode { get; set; }
        public int? Preferredappointmentdaycode { get; set; }
        public int? Preferredappointmenttimecode { get; set; }
        public int? Preferredcontactmethodcode { get; set; }
        public Guid? Processid { get; set; }
        public int? PtasAgeattimeofdeath { get; set; }
        public string PtasAlternatecontact { get; set; }
        public int? PtasCurrentage { get; set; }
        public string PtasDateofbirthstring { get; set; }
        public DateTime? PtasDateofdeath { get; set; }
        public bool? PtasDeathcertificate { get; set; }
        public string PtasLegacycreatedby { get; set; }
        public DateTimeOffset? PtasLegacycreatedon { get; set; }
        public string PtasLegacymodifiedby { get; set; }
        public DateTimeOffset? PtasLegacymodifiedon { get; set; }
        public bool? PtasObituary { get; set; }
        public string PtasObituarycity { get; set; }
        public int? PtasObituarysource { get; set; }
        public string PtasObituarystate { get; set; }
        public int? PtasSealternatekey { get; set; }
        public string PtasSeniorexemptionscorrespondencere { get; set; }
        public string PtasSpousefirstname { get; set; }
        public string PtasSpouselastname { get; set; }
        public string PtasSpousemiddlename { get; set; }
        public string PtasSpousesuffix { get; set; }
        public DateTimeOffset? PtasSurvivingspouseinforeceivedon { get; set; }
        public DateTimeOffset? PtasSurvivingspouseinfosenton { get; set; }
        public bool? PtasTextsmscapable { get; set; }
        public string Salutation { get; set; }
        public int? Shippingmethodcode { get; set; }
        public string Spousesname { get; set; }
        public Guid? Stageid { get; set; }
        public int? Statecode { get; set; }
        public int? Statuscode { get; set; }
        public Guid? Subscriptionid { get; set; }
        public string Suffix { get; set; }
        public int? Teamsfollowed { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Telephone3 { get; set; }
        public int? Territorycode { get; set; }
        public string Timespentbymeonemailandmeetings { get; set; }
        public int? Timezoneruleversionnumber { get; set; }
        public string Traversedpath { get; set; }
        public int? Utcconversiontimezonecode { get; set; }
        public long? Versionnumber { get; set; }
        public string Websiteurl { get; set; }
        public string Yomifirstname { get; set; }
        public string Yomifullname { get; set; }
        public string Yomilastname { get; set; }
        public string Yomimiddlename { get; set; }
        public Guid? AccountidValue { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedbyexternalpartyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? DefaultpricelevelidValue { get; set; }
        public Guid? MasteridValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedbyexternalpartyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? OriginatingleadidValue { get; set; }
        public Guid? OwneridValue { get; set; }
        public Guid? OwningbusinessunitValue { get; set; }
        public Guid? OwningteamValue { get; set; }
        public Guid? OwninguserValue { get; set; }
        public Guid? ParentcontactidValue { get; set; }
        public Guid? ParentcustomeridValue { get; set; }
        public Guid? PreferredequipmentidValue { get; set; }
        public Guid? PreferredserviceidValue { get; set; }
        public Guid? PreferredsystemuseridValue { get; set; }
        public Guid? PtasParcelidValue { get; set; }
        public Guid? SlaidValue { get; set; }
        public Guid? SlainvokedidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual Systemuser CreatedbyValueNavigation { get; set; }
        public virtual Systemuser CreatedonbehalfbyValueNavigation { get; set; }
        public virtual Contact MasteridValueNavigation { get; set; }
        public virtual Systemuser ModifiedbyValueNavigation { get; set; }
        public virtual Systemuser ModifiedonbehalfbyValueNavigation { get; set; }
        public virtual Team OwningteamValueNavigation { get; set; }
        public virtual Systemuser OwninguserValueNavigation { get; set; }
        public virtual Contact ParentcustomeridValueNavigation { get; set; }
        public virtual Systemuser PreferredsystemuseridValueNavigation { get; set; }
        public virtual PtasParceldetail PtasParcelidValueNavigation { get; set; }
        public virtual ICollection<Contact> InverseMasteridValueNavigation { get; set; }
        public virtual ICollection<Contact> InverseParentcustomeridValueNavigation { get; set; }
        public virtual ICollection<PtasCurrentuseapplication> PtasCurrentuseapplication { get; set; }
        public virtual ICollection<PtasExemption> PtasExemption { get; set; }
        public virtual ICollection<PtasSeappdetail> PtasSeappdetail { get; set; }
        public virtual ICollection<PtasSeapplication> PtasSeapplication { get; set; }
        public virtual ICollection<PtasSeapplicationtask> PtasSeapplicationtask { get; set; }
        public virtual ICollection<PtasSeappnote> PtasSeappnote { get; set; }
        public virtual ICollection<PtasSefrozenvalue> PtasSefrozenvalue { get; set; }
    }
}

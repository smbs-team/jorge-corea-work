using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesEFLibrary.CustomSearches.Model
{
    public partial class Systemuser
    {
        public Systemuser()
        {
            ChartTemplateCreatedByNavigation = new HashSet<ChartTemplate>();
            ChartTemplateLastModifiedByNavigation = new HashSet<ChartTemplate>();
            DatasetCreatedByNavigation = new HashSet<Dataset>();
            DatasetLastExecutedByNavigation = new HashSet<Dataset>();
            DatasetLastModifiedByNavigation = new HashSet<Dataset>();
            DatasetPostProcessCreatedByNavigation = new HashSet<DatasetPostProcess>();
            DatasetPostProcessLastModifiedByNavigation = new HashSet<DatasetPostProcess>();
            DatasetUser = new HashSet<Dataset>();
            Folder = new HashSet<Folder>();
            InteractiveChartCreatedByNavigation = new HashSet<InteractiveChart>();
            InteractiveChartLastModifiedByNavigation = new HashSet<InteractiveChart>();
            UserDataStoreItem = new HashSet<UserDataStoreItem>();
            UserJobNotification = new HashSet<UserJobNotification>();
            UserProjectCreatedByNavigation = new HashSet<UserProject>();
            UserProjectLastModifiedByNavigation = new HashSet<UserProject>();
            UserProjectUser = new HashSet<UserProject>();
        }

        public int? Accessmode { get; set; }
        public Guid? Address1Addressid { get; set; }
        public int? Address1Addresstypecode { get; set; }
        public string Address1City { get; set; }
        public string Address1Composite { get; set; }
        public string Address1Country { get; set; }
        public string Address1County { get; set; }
        public string Address1Fax { get; set; }
        public double? Address1Latitude { get; set; }
        public string Address1Line1 { get; set; }
        public string Address1Line2 { get; set; }
        public string Address1Line3 { get; set; }
        public double? Address1Longitude { get; set; }
        public string Address1Name { get; set; }
        public string Address1Postalcode { get; set; }
        public string Address1Postofficebox { get; set; }
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
        public double? Address2Latitude { get; set; }
        public string Address2Line1 { get; set; }
        public string Address2Line2 { get; set; }
        public string Address2Line3 { get; set; }
        public double? Address2Longitude { get; set; }
        public string Address2Name { get; set; }
        public string Address2Postalcode { get; set; }
        public string Address2Postofficebox { get; set; }
        public int? Address2Shippingmethodcode { get; set; }
        public string Address2Stateorprovince { get; set; }
        public string Address2Telephone1 { get; set; }
        public string Address2Telephone2 { get; set; }
        public string Address2Telephone3 { get; set; }
        public string Address2Upszone { get; set; }
        public int? Address2Utcoffset { get; set; }
        public Guid? Applicationid { get; set; }
        public string Applicationiduri { get; set; }
        public Guid? Azureactivedirectoryobjectid { get; set; }
        public int? Caltype { get; set; }
        public DateTimeOffset? Createdon { get; set; }
        public bool? Defaultfilterspopulated { get; set; }
        public string Defaultodbfoldername { get; set; }
        public string Disabledreason { get; set; }
        public bool? Displayinserviceviews { get; set; }
        public string Domainname { get; set; }
        public int? Emailrouteraccessapproval { get; set; }
        public string Employeeid { get; set; }
        public long? EntityimageTimestamp { get; set; }
        public string EntityimageUrl { get; set; }
        public Guid? Entityimageid { get; set; }
        public decimal? Exchangerate { get; set; }
        public string Firstname { get; set; }
        public string Fullname { get; set; }
        public string Governmentid { get; set; }
        public string Homephone { get; set; }
        public int? Identityid { get; set; }
        public int? Importsequencenumber { get; set; }
        public int? Incomingemaildeliverymethod { get; set; }
        public string Internalemailaddress { get; set; }
        public int? Invitestatuscode { get; set; }
        public bool? Isdisabled { get; set; }
        public bool? Isemailaddressapprovedbyo365admin { get; set; }
        public bool? Isintegrationuser { get; set; }
        public bool? Islicensed { get; set; }
        public bool? Issyncwithdirectory { get; set; }
        public string Jobtitle { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public string Mobilealertemail { get; set; }
        public string Mobilephone { get; set; }
        public DateTimeOffset? Modifiedon { get; set; }
        public bool? MsdynGdproptout { get; set; }
        public string Nickname { get; set; }
        public Guid? Organizationid { get; set; }
        public int? Outgoingemaildeliverymethod { get; set; }
        public DateTimeOffset? Overriddencreatedon { get; set; }
        public int? Passporthi { get; set; }
        public int? Passportlo { get; set; }
        public string Personalemailaddress { get; set; }
        public string Photourl { get; set; }
        public int? Preferredaddresscode { get; set; }
        public int? Preferredemailcode { get; set; }
        public int? Preferredphonecode { get; set; }
        public Guid? Processid { get; set; }
        public string PtasLegacyid { get; set; }
        public string Salutation { get; set; }
        public bool? Setupuser { get; set; }
        public string Sharepointemailaddress { get; set; }
        public string Skills { get; set; }
        public Guid? Stageid { get; set; }
        public Guid Systemuserid { get; set; }
        public int? Timezoneruleversionnumber { get; set; }
        public string Title { get; set; }
        public string Traversedpath { get; set; }
        public int? Userlicensetype { get; set; }
        public string Userpuid { get; set; }
        public int? Utcconversiontimezonecode { get; set; }
        public long? Versionnumber { get; set; }
        public string Windowsliveid { get; set; }
        public string Yammeremailaddress { get; set; }
        public string Yammeruserid { get; set; }
        public string Yomifirstname { get; set; }
        public string Yomifullname { get; set; }
        public string Yomilastname { get; set; }
        public string Yomimiddlename { get; set; }
        public Guid? BusinessunitidValue { get; set; }
        public Guid? CalendaridValue { get; set; }
        public Guid? CreatedbyValue { get; set; }
        public Guid? CreatedonbehalfbyValue { get; set; }
        public Guid? DefaultmailboxValue { get; set; }
        public Guid? MobileofflineprofileidValue { get; set; }
        public Guid? ModifiedbyValue { get; set; }
        public Guid? ModifiedonbehalfbyValue { get; set; }
        public Guid? ParentsystemuseridValue { get; set; }
        public Guid? PositionidValue { get; set; }
        public Guid? QueueidValue { get; set; }
        public Guid? SiteidValue { get; set; }
        public Guid? TerritoryidValue { get; set; }
        public Guid? TransactioncurrencyidValue { get; set; }

        public virtual ICollection<ChartTemplate> ChartTemplateCreatedByNavigation { get; set; }
        public virtual ICollection<ChartTemplate> ChartTemplateLastModifiedByNavigation { get; set; }
        public virtual ICollection<Dataset> DatasetCreatedByNavigation { get; set; }
        public virtual ICollection<Dataset> DatasetLastExecutedByNavigation { get; set; }
        public virtual ICollection<Dataset> DatasetLastModifiedByNavigation { get; set; }
        public virtual ICollection<DatasetPostProcess> DatasetPostProcessCreatedByNavigation { get; set; }
        public virtual ICollection<DatasetPostProcess> DatasetPostProcessLastModifiedByNavigation { get; set; }
        public virtual ICollection<Dataset> DatasetUser { get; set; }
        public virtual ICollection<Folder> Folder { get; set; }
        public virtual ICollection<InteractiveChart> InteractiveChartCreatedByNavigation { get; set; }
        public virtual ICollection<InteractiveChart> InteractiveChartLastModifiedByNavigation { get; set; }
        public virtual ICollection<UserDataStoreItem> UserDataStoreItem { get; set; }
        public virtual ICollection<UserJobNotification> UserJobNotification { get; set; }
        public virtual ICollection<UserProject> UserProjectCreatedByNavigation { get; set; }
        public virtual ICollection<UserProject> UserProjectLastModifiedByNavigation { get; set; }
        public virtual ICollection<UserProject> UserProjectUser { get; set; }
    }
}

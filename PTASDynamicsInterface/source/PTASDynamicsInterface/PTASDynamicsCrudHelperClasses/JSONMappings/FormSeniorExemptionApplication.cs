// <copyright file="FormSeniorExemptionApplication.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;

    using Newtonsoft.Json;

    using PTASDynamicsCrudHelperClasses.Classes;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Senior Exemption Application to be read from API.
    /// </summary>
    public class FormSeniorExemptionApplication : FormInput, ISeniorExemptionApplication
    {
        /// <inheritdoc/>
        [JsonProperty("seapplicationid")]
        public string SEAapplicationId { get; set; } = null;

        /// <inheritdoc/>
        [JsonProperty("alternatekey")]
        public int? AlternateKey { get; set; }

        /// <inheritdoc/>
        [JsonProperty("applicationtype")]
        public int? ApplicationType { get; set; }

        /// <inheritdoc/>
        [JsonProperty("source")]
        public int? Source { get; set; }

        /// <inheritdoc/>
        [JsonProperty("splitcode")]
        public int? SplitCode { get; set; }

        /// <inheritdoc/>
        [JsonProperty("spousedob")]
        public DateTime? SpouseDOB { get; set; }

        /// <inheritdoc/>
        [JsonProperty("statuschangedate")]
        public DateTime? StatusChangeDate { get; set; }

        /// <inheritdoc/>
        [JsonProperty("vadisabled")]
        public bool? VaDisabled { get; set; }

        /// <inheritdoc/>
        [JsonProperty("parcelid", NullValueHandling = NullValueHandling.Ignore)]
        public string ParcelId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ownmultipleproperties")]
        public bool? OwnMultipleProperties { get; set; }

        /// <inheritdoc/>
        [JsonProperty("occupieddate")]
        public DateTime? OccupiedDate { get; set; }

        /// <inheritdoc/>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <inheritdoc/>
        [JsonProperty("lifeestate")]
        public bool? LifeEstate { get; set; }

        /// <inheritdoc/>
        [JsonProperty("docspouse")]
        public bool? DocSpouse { get; set; }

        /// <inheritdoc/>
        [JsonProperty("docpassport")]
        public bool? DocPassport { get; set; }

        /// <inheritdoc/>
        [JsonProperty("docother")]
        public string DocOther { get; set; }

        /// <inheritdoc/>
        [JsonProperty("docdriverslicense")]
        public bool? DocDriversLicense { get; set; }

        /// <inheritdoc/>
        [JsonProperty("doccotenant")]
        public bool? DocCotenant { get; set; }

        /// <inheritdoc/>
        [JsonProperty("docbirthcertificate")]
        public bool? DocBirthCertificate { get; set; }

        /// <inheritdoc/>
        [JsonProperty("disabled")]
        public bool? Disabled { get; set; }

        /// <inheritdoc/>
        [JsonProperty("dayslaststatuschange")]
        public int? DaysLastStatusChange { get; set; }

        /// <inheritdoc/>
        [JsonProperty("datereceived")]
        public DateTime? DateReceived { get; set; }

        /// <inheritdoc/>
        [JsonProperty("currentlyownoccupy")]
        public bool? CurrentlyOwnOccupy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("contactid", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("applicationdate")]
        public DateTime? ApplicationDate { get; set; }

        /// <inheritdoc/>
        [JsonProperty("accounttype")]
        public int? AccountType { get; set; }

        /// <inheritdoc/>
        [JsonProperty("accountnumber")]
        public string AccountNumber { get; set; }

        /// <inheritdoc/>
        [JsonProperty("hadexinanothercounty")]
        public bool? HadExInAnotherCounty { get; set; }

        /// <inheritdoc/>
        [JsonProperty("transferredfrcounty", NullValueHandling = NullValueHandling.Ignore)]
        public string TransferredFrCounty { get; set; }

        /// <inheritdoc/>
        [JsonProperty("otheroccupants")]
        public bool? OtherOccupants { get; set; }

        /// <inheritdoc/>
        [JsonProperty("otherowners")]
        public string OtherOwners { get; set; }

        /// <inheritdoc/>
        [JsonProperty("createdon")]
        public DateTime? CreatedOn { get; set; }

        /// <inheritdoc/>
        [JsonProperty("modifiedon")]
        public DateTime? ModifiedOn { get; set; }

        /// <inheritdoc/>
        [JsonProperty("statuscode")]
        public int? StatusCode { get; set; }

        /// <inheritdoc/>
        [JsonProperty("statecode")]
        public int? StateCode { get; set; }

        /// <inheritdoc/>
        [JsonProperty("modifiedby", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? ModifiedBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("createdby", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? CreatedBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("createdonbehalfby", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? CreatedOnBehalfBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("veteran")]
        public bool? IsVeteran { get; set; }

        /// <inheritdoc/>
        [JsonProperty("taxpayersection")]
        public bool? TaxpayerSection { get; set; }

        /// <inheritdoc/>
        [JsonProperty("spousefirstname")]
        public string SpouseFirstName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("spousemiddlename")]
        public string SpouseMiddleName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("spouselastname")]
        public string SpouseLastName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("spousesuffix")]
        public string SpouseSuffix { get; set; }

        /// <inheritdoc/>
        [JsonProperty("deceasedspouseex")]
        public bool? DeceasedSpouseEx { get; set; }

        /// <inheritdoc/>
        [JsonProperty("firstdateprimaryres")]
        public DateTime? FirstDatePrimaryRes { get; set; }

        /// <inheritdoc/>
        [JsonProperty("majorlifechange")]
        public bool? MajorLifeChange { get; set; }

        /// <inheritdoc/>
        [JsonProperty("addrcountryid", NullValueHandling = NullValueHandling.Include)]
        public string AddrcountryId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("coopproperty")]
        public bool? CoopProperty { get; set; }

        /// <inheritdoc/>
        [JsonProperty("coopname")]
        public string CoopName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("cooptreasurer")]
        public string CoopTreasurer { get; set; }

        /// <inheritdoc/>
        [JsonProperty("cooptreasurerphone")]
        public string CoopRreasurerPhone { get; set; }

        /// <inheritdoc/>
        [JsonProperty("coopownedshares")]
        public string CoopOwnedShares { get; set; }

        /// <inheritdoc/>
        [JsonProperty("cooptotalshares")]
        public string CoopTotalShares { get; set; }

        /// <inheritdoc/>
        [JsonProperty("addrchange")]
        public bool? AddrChange { get; set; }

        /// <inheritdoc/>
        [JsonProperty("addrstreet1")]
        public string AddrStreet1 { get; set; }

        /// <inheritdoc/>
        [JsonProperty("addrcity")]
        public string AddrCity { get; set; }

        /// <inheritdoc/>
        [JsonProperty("addrstate")]
        public string AddrState { get; set; }

        /// <inheritdoc/>
        [JsonProperty("addrpostal")]
        public string AddrPostal { get; set; }

        /// <inheritdoc/>
        [JsonProperty("propertysection")]
        public bool? PropertySection { get; set; }

        /// <inheritdoc/>
        [JsonProperty("financialsection")]
        public bool? FinancialSection { get; set; }

        /// <inheritdoc/>
        [JsonProperty("signatureconfirmed")]
        public bool? SignatureConfirmed { get; set; }

        /// <inheritdoc/>
        [JsonProperty("signatureline")]
        public string SignatureLine { get; set; }

        /// <inheritdoc/>
        [JsonProperty("signaturedate")]
        public DateTime? SignatureDate { get; set; }

        /// <inheritdoc/>
        [JsonProperty("signaturesection")]
        public bool? SignatureSection { get; set; }

        /// <inheritdoc/>
        [JsonProperty("docageapplicant")]
        public bool? DocAgeApplicant { get; set; }

        /// <inheritdoc/>
        [JsonProperty("docspouseage")]
        public bool? DocSpouseAge { get; set; }

        /// <inheritdoc/>
        [JsonProperty("docdisability")]
        public bool? DocDisability { get; set; }

        /// <inheritdoc/>
        [JsonProperty("docownership")]
        public bool? DocOwnership { get; set; }

        /// <inheritdoc/>
        [JsonProperty("docmajorlifechange")]
        public bool? DocMajorLifechange { get; set; }

        /// <inheritdoc/>
        [JsonProperty("doccoopshares")]
        public bool? DocCoopShares { get; set; }

        /// <inheritdoc/>
        [JsonProperty("missingdocumentlist")]
        public string MissingDocumentList { get; set; }

        /// <inheritdoc/>
        [JsonProperty("applicantdateofbirth")]
        public DateTime? DateofBirth { get; set; }

        /// <inheritdoc/>
        [JsonProperty("applicantemailaddress")]
        public string EmailAddress { get; set; }

        /// <inheritdoc/>
        [JsonProperty("applicantfirstname")]
        public string FirstName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("applicantlastname")]
        public string LastName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("applicantmiddlename")]
        public string MiddleName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("applicantmobilephone")]
        public string MobilePhone { get; set; }

        /// <inheritdoc/>
        [JsonProperty("applicantphonenumber")]
        public string PhoneNumber { get; set; }

        /// <inheritdoc/>
        [JsonProperty("applicantsuffix")]
        public string Suffix { get; set; }

        /// <inheritdoc/>
        [JsonProperty("hasspouseorpartner")]
        public bool? HasSpouseOrPartner { get; set; }

        /// <inheritdoc/>
        [JsonProperty("residingfor9months")]
        public bool? ResidingFor9Months { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ownthroughtrust")]
        public bool? OwnThroughTrust { get; set; }

        /// <inheritdoc/>
        [JsonProperty("othercountyaddress")]
        public string OtherCountyAddress { get; set; }

        /// <inheritdoc/>
        [JsonProperty("othercountycity")]
        public string OtherCountyCity { get; set; }

        /// <inheritdoc/>
        [JsonProperty("othercountystate")]
        public string OtherCountyState { get; set; }

        /// <inheritdoc/>
        [JsonProperty("othercountypostal")]
        public string OtherCountyPostal { get; set; }

        /// <inheritdoc/>
        [JsonProperty("checkaddresscity")]
        public string CheckAddressCity { get; set; }

        /// <inheritdoc/>
        [JsonProperty("checkaddresscountryid", NullValueHandling = NullValueHandling.Ignore)]
        public string CheckAddressCountryId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("checkaddressname")]
        public string CheckAddressName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("checkaddresspostalcode")]
        public string CheckAddressPostalCode { get; set; }

        /// <inheritdoc/>
        [JsonProperty("checkaddressstate")]
        public string CheckAddressState { get; set; }

        /// <inheritdoc/>
        [JsonProperty("checkaddressstreet")]
        public string CheckAddressStreet { get; set; }

        /// <inheritdoc/>
        [JsonProperty("correspondencename")]
        public string CorrespondenceName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("differentcheckaddress")]
        public bool? DifferentCheckAddress { get; set; }

        /// <inheritdoc/>
        [JsonProperty("otherparcelnumber")]
        public string OtherParcelNumber { get; set; }

        /// <inheritdoc/>
        [JsonProperty("propertytaxesdelinquent")]
        public bool? PropertyTaxesDelinquent { get; set; }

        /// <inheritdoc/>
        [JsonIgnore]
        public ISEAppPredefNote[] SEApplicationPredefNotes
        {
            get
            {
                return this.SEApplicationPredefNotesCollection;
            }

            set
            {
                this.SEApplicationPredefNotesCollection = value as FormSEAppPredefNote[];
            }
        }

        /// <summary>
        /// Gets or Sets the Application Predefined notes.
        /// </summary>
        [JsonProperty("seApplicationPredefNotes")]
        public FormSEAppPredefNote[] SEApplicationPredefNotesCollection { get; set; }

        /// <inheritdoc/>
        [JsonProperty("requiredtofilefederalincometaxreturn")]
        public bool? RequiredToFileFederalIncomeTaxReturn { get; set; }

        /// <inheritdoc/>
        [JsonProperty("under61withdisabilitynotice")]
        public bool? Under61WithDisabilityNotice { get; set; }

        /// <inheritdoc/>
        [JsonProperty("veteranwithserviceevaluationordisability")]
        public bool? VeteranWithServiceEvaluationOrDisability { get; set; }

        /// <inheritdoc/>
        [JsonProperty("isasurvivingspouse")]
        public bool? IsASurvivingSpouse { get; set; }

        /// <inheritdoc/>
        [JsonProperty("married")]
        public bool? Married { get; set; }

        /// <inheritdoc/>
        [JsonProperty("single")]
        public bool? Single { get; set; }

        /// <inheritdoc/>
        [JsonProperty("widowed")]
        public bool? Widowed { get; set; }

        /// <inheritdoc/>
        [JsonProperty("divorcedlegallyseparated")]
        public bool? DivorcedLegallySeparated { get; set; }

        /// <inheritdoc/>
        [JsonProperty("marriedlivingapart")]
        public bool? MarriedLivingApart { get; set; }

        /// <inheritdoc/>
        [JsonProperty("propertyusedforbusiness")]
        public bool? PropertyUsedForBusiness { get; set; }

        /// <inheritdoc/>
        [JsonProperty("percentageusedforbusiness")]
        public double? PercentageUsedForBusiness { get; set; }

        /// <inheritdoc/>
        [JsonProperty("squarefootageusedforbusiness")]
        public int? SquareFootageUsedForBusiness { get; set; }

        /// <inheritdoc/>
        [JsonProperty("rentoutaportionofproperty")]
        public bool? RentoutAPortionOfProperty { get; set; }

        /// <inheritdoc/>
        [JsonProperty("percentagerentedout")]
        public double? PercentageRentedOut { get; set; }

        /// <inheritdoc/>
        [JsonProperty("squarefootagerentedout")]
        public int? SquareFootageRentedOut { get; set; }

        /// <inheritdoc/>
        [JsonProperty("datepropertypurchased")]
        public DateTime? DatePropertyPurchased { get; set; }

        /// <inheritdoc/>
        [JsonProperty("singlefamilyresidence")]
        public bool? SingleFamilyResidence { get; set; }

        /// <inheritdoc/>
        [JsonProperty("housingcoop")]
        public bool? HousingCoOp { get; set; }

        /// <inheritdoc/>
        [JsonProperty("singleunitofmultidwellingcondoorduplex")]
        public bool? SingleUnitOfMultiDwellingCondoOrDuplex { get; set; }

        /// <inheritdoc/>
        [JsonProperty("mobilehome")]
        public bool? MobileHome { get; set; }

        /// <inheritdoc/>
        [JsonProperty("mobilehomeyear")]
        public string MobileHomeYear { get; set; }

        /// <inheritdoc/>
        [JsonProperty("mobilehomemake")]
        public string MobileHomeMake { get; set; }

        /// <inheritdoc/>
        [JsonProperty("mobilehomemodel")]
        public string MobileHomeModel { get; set; }

        /// <inheritdoc/>
        [JsonProperty("receivedexemptionbefore")]
        public bool? ReceivedExemptionBefore { get; set; }

        /// <inheritdoc/>
        [JsonProperty("whenwasthepreviousexemption")]
        public DateTime? WhenWasThePreviousExemption { get; set; }

        /// <inheritdoc/>
        [JsonProperty("wherewasthepreviousexemption")]
        public string WhereWasThePreviousExemption { get; set; }

        /// <inheritdoc/>
        [JsonProperty("soldformerresidence")]
        public bool? SoldFormerResidence { get; set; }

        /// <inheritdoc/>
        [JsonProperty("dateofformerpropertysale")]
        public DateTime? DateOfFormerPropertySale { get; set; }

        /// <inheritdoc/>
        [JsonProperty("soldotherpropertyin2020")]
        public bool? SoldOtherPropertyIn2020 { get; set; }

        /// <inheritdoc/>
        [JsonProperty("wherepropertywassoldin2020")]
        public string WherePropertyWasSoldIn2020 { get; set; }

        /// <inheritdoc/>
        [JsonProperty("datepropertysoldin2020")]
        public DateTime? DatePropertySoldIn2020 { get; set; }

        /// <inheritdoc/>
        [JsonProperty("effectivedateofdisability")]
        public DateTime? EffectiveDateOfDisability { get; set; }

        /// <summary>
        /// Sets the id for a new contact.
        /// </summary>
        public override void SetId()
        {
            if (string.IsNullOrEmpty(this.SEAapplicationId))
            {
                this.SEAapplicationId = Guid.NewGuid().ToString();
            }
        }
    }
}
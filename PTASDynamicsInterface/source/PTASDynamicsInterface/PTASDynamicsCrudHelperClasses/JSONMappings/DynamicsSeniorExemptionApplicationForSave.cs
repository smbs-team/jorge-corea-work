// <copyright file="DynamicsSeniorExemptionApplicationForSave.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;
    using Newtonsoft.Json;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Senior Exemption Application info returned by dynamics.
    /// </summary>
    public class DynamicsSeniorExemptionApplicationForSave : ISeniorExemptionApplication
    {
        /// <inheritdoc/>
        [JsonProperty("ptas_seapplicationid")]
        public string SEAapplicationId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_alternatekey")]
        [JsonConverter(typeof(IntegerValidationConverter))]
        public int? AlternateKey { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_applicationtype")]
        public int? ApplicationType { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_source")]
        public int? Source { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_splitcode")]
        public int? SplitCode { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_spousedob")]
        [JsonConverter(typeof(ShortNullableDateConverter))]
        public DateTime? SpouseDOB { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_statuschangedate")]
        public DateTime? StatusChangeDate { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_vadisabled")]
        public bool? VaDisabled { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_parcelId@odata.bind", NullValueHandling = NullValueHandling.Ignore)]
        public string ParcelId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_ownmultipleproperties")]
        public bool? OwnMultipleProperties { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_occupieddate")]
        public DateTime? OccupiedDate { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_name")]
        public string Name { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_lifeestate")]
        public bool? LifeEstate { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docspouse")]
        public bool? DocSpouse { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docpassport")]
        public bool? DocPassport { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docother")]
        public string DocOther { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docdriverslicense")]
        public bool? DocDriversLicense { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_doccotenant")]
        public bool? DocCotenant { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docbirthcertificate")]
        public bool? DocBirthCertificate { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_disabled")]
        public bool? Disabled { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_dayslaststatuschange")]
        public int? DaysLastStatusChange { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_datereceived")]
        [JsonConverter(typeof(ShortNullableDateConverter))]
        public DateTime? DateReceived { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_currentlyownoccupy")]
        public bool? CurrentlyOwnOccupy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_contactid@odata.bind", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_applicationdate")]
        [JsonConverter(typeof(ShortNullableDateConverter))]
        public DateTime? ApplicationDate { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_accounttype")]
        public int? AccountType { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_accountnumber")]
        public string AccountNumber { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_hadexinanothercounty")]
        public bool? HadExInAnotherCounty { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_transferredfrcounty@odata.bind", NullValueHandling = NullValueHandling.Ignore)]
        public string TransferredFrCounty { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_otheroccupants")]
        public bool? OtherOccupants { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_otherowners")]
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
        [JsonProperty("statecode", NullValueHandling = NullValueHandling.Ignore)]
        public int? StateCode { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_modifiedby_value", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? ModifiedBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_createdby_value", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? CreatedBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_createdonbehalfby_value", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? CreatedOnBehalfBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_veteran")]
        public bool? IsVeteran { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_taxpayersection")]
        public bool? TaxpayerSection { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_spousefirstname")]
        public string SpouseFirstName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_spousemiddlename")]
        public string SpouseMiddleName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_spouselastname")]
        public string SpouseLastName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_spousesuffix")]
        public string SpouseSuffix { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_deceasedspouseex")]
        public bool? DeceasedSpouseEx { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_firstdateprimaryres")]
        [JsonConverter(typeof(ShortNullableDateConverter))]
        public DateTime? FirstDatePrimaryRes { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_majorlifechange")]
        public bool? MajorLifeChange { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_addrcountryid@odata.bind", NullValueHandling = NullValueHandling.Ignore)]
        public string AddrcountryId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_coopproperty")]
        public bool? CoopProperty { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_coopname")]
        public string CoopName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_cooptreasurer")]
        public string CoopTreasurer { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_cooptreasurerphone")]
        public string CoopRreasurerPhone { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_coopownedshares")]
        public string CoopOwnedShares { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_cooptotalshares")]
        public string CoopTotalShares { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_addrchange")]
        public bool? AddrChange { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_addrstreet1")]
        public string AddrStreet1 { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_addrcity")]
        public string AddrCity { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_addrstate")]
        public string AddrState { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_addrpostal")]
        public string AddrPostal { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_propertysection")]
        public bool? PropertySection { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_financialsection")]
        public bool? FinancialSection { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_signatureconfirmed")]
        public bool? SignatureConfirmed { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_signatureline")]
        public string SignatureLine { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_signaturedate")]
        public DateTime? SignatureDate { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_signaturesection")]
        public bool? SignatureSection { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docageapplicant")]
        public bool? DocAgeApplicant { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docspouseage")]
        public bool? DocSpouseAge { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docdisability")]
        public bool? DocDisability { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docownership")]
        public bool? DocOwnership { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docmajorlifechange")]
        public bool? DocMajorLifechange { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_doccoopshares")]
        public bool? DocCoopShares { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_missingdocumentlist")]
        public string MissingDocumentList { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_applicantdateofbirth")]
        [JsonConverter(typeof(ShortNullableDateConverter))]
        public DateTime? DateofBirth { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_applicantemailaddress")]
        public string EmailAddress { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_applicantfirstname")]
        public string FirstName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_applicantlastname")]
        public string LastName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_applicantmiddlename")]
        public string MiddleName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_applicantmobilephone")]
        public string MobilePhone { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_applicantphonenumber")]
        public string PhoneNumber { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_applicantsuffix")]
        public string Suffix { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_hasspouseorpartner")]
        public bool? HasSpouseOrPartner { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_residingfor9months")]
        public bool? ResidingFor9Months { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_ownthroughtrust")]
        public bool? OwnThroughTrust { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_othercountyaddress")]
        public string OtherCountyAddress { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_othercountycity")]
        public string OtherCountyCity { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_othercountystate")]
        public string OtherCountyState { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_othercountypostal")]
        public string OtherCountyPostal { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_checkaddresscity")]
        public string CheckAddressCity { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_checkaddresscountryid@odata.bind", NullValueHandling = NullValueHandling.Ignore)]
        public string CheckAddressCountryId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_checkaddressname")]
        public string CheckAddressName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_checkaddresspostalcode")]
        public string CheckAddressPostalCode { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_checkaddressstate")]
        public string CheckAddressState { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_checkaddressstreet")]
        public string CheckAddressStreet { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_correspondencename")]
        public string CorrespondenceName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_differentcheckaddress")]
        public bool? DifferentCheckAddress { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_otherparcelnumber")]
        public string OtherParcelNumber { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_propertytaxesdelinquent", NullValueHandling = NullValueHandling.Ignore)]
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
                this.SEApplicationPredefNotesCollection = value as DynamicsSEAppPredefNote[];
            }
        }

        /// <summary>
        /// Gets or Sets the Application Predefined Notes.
        /// </summary>
        [JsonIgnore]
        public DynamicsSEAppPredefNote[] SEApplicationPredefNotesCollection { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_requiredtofilefederalincometaxreturn")]
        public bool? RequiredToFileFederalIncomeTaxReturn { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_under61withdisabilitynotice")]
        public bool? Under61WithDisabilityNotice { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_veteranwithserviceevaluationordisability")]
        public bool? VeteranWithServiceEvaluationOrDisability { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_isasurvivingspouse")]
        public bool? IsASurvivingSpouse { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_married")]
        public bool? Married { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_single")]
        public bool? Single { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_widowed")]
        public bool? Widowed { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_divorcedlegallyseparated")]
        public bool? DivorcedLegallySeparated { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_marriedlivingapart")]
        public bool? MarriedLivingApart { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_propertyusedforbusiness")]
        public bool? PropertyUsedForBusiness { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_percentageusedforbusiness")]
        [JsonConverter(typeof(CurrencyValidationConverter))]
        public double? PercentageUsedForBusiness { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_squarefootageusedforbusiness")]
        [JsonConverter(typeof(IntegerValidationConverter))]
        public int? SquareFootageUsedForBusiness { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_rentoutaportionofproperty")]
        public bool? RentoutAPortionOfProperty { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_percentagerentedout")]
        [JsonConverter(typeof(CurrencyValidationConverter))]
        public double? PercentageRentedOut { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_squarefootagerentedout")]
        [JsonConverter(typeof(IntegerValidationConverter))]
        public int? SquareFootageRentedOut { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_datepropertypurchased")]
        [JsonConverter(typeof(ShortNullableDateConverter))]
        public DateTime? DatePropertyPurchased { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_singlefamilyresidence")]
        public bool? SingleFamilyResidence { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_housingcoop")]
        public bool? HousingCoOp { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_singleunitofmultidwellingcondoorduplex")]
        public bool? SingleUnitOfMultiDwellingCondoOrDuplex { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_mobilehome")]
        public bool? MobileHome { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_mobilehomeyear")]
        public string MobileHomeYear { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_mobilehomemake")]
        public string MobileHomeMake { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_mobilehomemodel")]
        public string MobileHomeModel { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_receivedexemptionbefore")]
        public bool? ReceivedExemptionBefore { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_whenwasthepreviousexemption")]
        [JsonConverter(typeof(ShortNullableDateConverter))]
        public DateTime? WhenWasThePreviousExemption { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_wherewasthepreviousexemption")]
        public string WhereWasThePreviousExemption { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_soldformerresidence")]
        public bool? SoldFormerResidence { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_dateofformerpropertysale")]
        [JsonConverter(typeof(ShortNullableDateConverter))]
        public DateTime? DateOfFormerPropertySale { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_soldotherpropertyin2020")]
        public bool? SoldOtherPropertyIn2020 { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_wherepropertywassoldin2020")]
        public string WherePropertyWasSoldIn2020 { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_datepropertysoldin2020")]
        [JsonConverter(typeof(ShortNullableDateConverter))]
        public DateTime? DatePropertySoldIn2020 { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_effectivedateofdisability")]
        [JsonConverter(typeof(ShortNullableDateConverter))]
        public DateTime? EffectiveDateOfDisability { get; set; }
    }
}

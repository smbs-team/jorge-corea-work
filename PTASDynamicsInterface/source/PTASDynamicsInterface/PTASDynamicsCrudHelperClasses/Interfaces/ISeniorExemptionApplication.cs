// <copyright file="ISeniorExemptionApplication.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    using System;

    /// <summary>
    /// Senior Exemption Application for crud.
    /// </summary>
    public interface ISeniorExemptionApplication
    {
        /// <summary>
        /// Gets or sets a value for SEAapplicationId.
        /// </summary>
        string SEAapplicationId { get; set; }

        /// <summary>
        /// Gets or sets a value for Alternate Key.
        /// </summary>
        int? AlternateKey { get; set; }

        /// <summary>
        /// Gets or sets a value for Application Type.
        /// </summary>
        int? ApplicationType { get; set; }

        /// <summary>
        /// Gets or sets a value for Source.
        /// </summary>
        int? Source { get; set; }

        /// <summary>
        /// Gets or sets a value for Split Code.
        /// </summary>
        int? SplitCode { get; set; }

        /// <summary>
        /// Gets or sets a value for Spouse DOB.
        /// </summary>
        DateTime? SpouseDOB { get; set; }

        /// <summary>
        /// Gets or sets a value for Status Change Date.
        /// </summary>
        DateTime? StatusChangeDate { get; set; }

        /// <summary>
        ///  Gets or sets a value for veterans disabled.
        /// </summary>
        bool? VaDisabled { get; set; }

        /// <summary>
        /// Gets or sets a value for Parcel Id.
        /// </summary>
        string ParcelId { get; set; }

        /// <summary>
        /// Gets or sets a value for OwnMultipleProperties.
        /// </summary>
        bool? OwnMultipleProperties { get; set; }

        /// <summary>
        /// Gets or sets a value for Occupied Date.
        /// </summary>
        DateTime? OccupiedDate { get; set; }

        /// <summary>
        /// Gets or sets a value for Name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets a value for LifeEstate.
        /// </summary>
        bool? LifeEstate { get; set; }

        /// <summary>
        /// Gets or sets a value for DocSpouse.
        /// </summary>
        bool? DocSpouse { get; set; }

        /// <summary>
        /// Gets or sets a value for DocPassport.
        /// </summary>
        bool? DocPassport { get; set; }

        /// <summary>
        /// Gets or sets a value for Doc Other.
        /// </summary>
        string DocOther { get; set; }

        /// <summary>
        /// Gets or sets a value for DocDriversLicense.
        /// </summary>
        bool? DocDriversLicense { get; set; }

        /// <summary>
        /// Gets or sets a value for DocCotenant.
        /// </summary>
        bool? DocCotenant { get; set; }

        /// <summary>
        /// Gets or sets a value for DocBirthCertificate.
        /// </summary>
        bool? DocBirthCertificate { get; set; }

        /// <summary>
        /// Gets or sets a value for Disabled.
        /// </summary>
        bool? Disabled { get; set; }

        /// <summary>
        /// Gets or sets a value for Days Last Status Change.
        /// </summary>
        int? DaysLastStatusChange { get; set; }

        /// <summary>
        /// Gets or sets a value for Date Received.
        /// </summary>
        DateTime? DateReceived { get; set; }

        /// <summary>
        /// Gets or sets a value for CurrentlyOwnOccupy.
        /// </summary>
        bool? CurrentlyOwnOccupy { get; set; }

        /// <summary>
        /// Gets or sets a value for ContactId.
        /// </summary>
        string ContactId { get; set; }

        /// <summary>
        /// Gets or sets a value for ApplicationDate.
        /// </summary>
        DateTime? ApplicationDate { get; set; }

        /// <summary>
        /// Gets or sets a value for Account Type.
        /// </summary>
        int? AccountType { get; set; }

        /// <summary>
        /// Gets or sets a value for Account Number.
        /// </summary>
        string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets a value for HadExInAnotherCounty.
        /// </summary>
        bool? HadExInAnotherCounty { get; set; }

        /// <summary>
        /// Gets or sets a value for Transferred From County.
        /// </summary>
        string TransferredFrCounty { get; set; }

        /// <summary>
        /// Gets or sets a value for OtherOccupants.
        /// </summary>
        bool? OtherOccupants { get; set; }

        /// <summary>
        ///  Gets or sets a value for Other Owners.
        /// </summary>
        string OtherOwners { get; set; }

        /// <summary>
        /// Gets or sets a value for Create On.
        /// </summary>
        DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets a value for Modified On.
        /// </summary>
        DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets a value for Status Code.
        /// </summary>
        int? StatusCode { get; set; }

        /// <summary>
        ///  Gets or sets a value for State Code.
        /// </summary>
        int? StateCode { get; set; }

        /// <summary>
        /// Gets or sets a value for Modified By.
        /// </summary>
        Guid? ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets a value for Created By.
        /// </summary>
        Guid? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets a value for Created On Behalf By.
        /// </summary>
        Guid? CreatedOnBehalfBy { get; set; }

        /// <summary>
        ///  Gets or sets a value for know if is I or my sousage or registered domestic partner is veteran.
        /// </summary>
        bool? IsVeteran { get; set; }

        /// <summary>
        ///  Gets or sets a value for know if Taxpayer Section Verified.
        /// </summary>
        bool? TaxpayerSection { get; set; }

        /// <summary>
        /// Gets or sets the first name of the spouse.
        /// </summary>
        string SpouseFirstName { get; set; }

        /// <summary>
        /// Gets or sets the middle name of the spouse.
        /// </summary>
        string SpouseMiddleName { get; set; }

        /// <summary>
        /// Gets or sets the middle name of the spouse.
        /// </summary>
        string SpouseLastName { get; set; }

        /// <summary>
        /// Gets or sets the suffix of the spouse.
        /// </summary>
        string SpouseSuffix { get; set; }

        /// <summary>
        /// Gets or sets Deceased Spouse/Domestic Partner had an Exemption.
        /// </summary>
        bool? DeceasedSpouseEx { get; set; }

        /// <summary>
        /// Gets or sets First Date as Primary Residence.
        /// </summary>
        DateTime? FirstDatePrimaryRes { get; set; }

        /// <summary>
        /// Gets or sets if there has Major Life Change.
        /// </summary>
        bool? MajorLifeChange { get; set; }

        /// <summary>
        /// Gets or sets the County.
        string AddrcountryId { get; set; }

        /// <summary>
        /// Gets or sets if Property is a Co-op.
        /// </summary>
        bool? CoopProperty { get; set; }

        /// <summary>
        /// Gets or sets Name of Co-op.
        /// </summary>
        string CoopName { get; set; }

        /// <summary>
        /// Gets or sets Treasurer.
        /// </summary>
        string CoopTreasurer { get; set; }

        /// <summary>
        /// Gets or sets Treasurer Phone Number.
        /// </summary>
        string CoopRreasurerPhone { get; set; }

        /// <summary>
        /// Gets or sets Number of Shares you Own.
        /// </summary>
        string CoopOwnedShares { get; set; }

        /// <summary>
        /// Gets or sets Total Co-op Shares.
        /// </summary>
        string CoopTotalShares { get; set; }

        /// <summary>
        /// Gets or sets if I need to change my correspondence address.
        /// </summary>
        bool? AddrChange { get; set; }

        /// <summary>
        /// Gets or sets Address Street1.
        /// </summary>
        string AddrStreet1 { get; set; }

        /// <summary>
        /// Gets or sets Address City.
        /// </summary>
        string AddrCity { get; set; }

        /// <summary>
        /// Gets or sets Address state.
        /// </summary>
        string AddrState { get; set; }

        /// <summary>
        /// Gets or sets Address postal.
        /// </summary>
        string AddrPostal { get; set; }

        /// <summary>
        /// Gets or sets Property Section Verified.
        /// </summary>
        bool? PropertySection { get; set; }

        /// <summary>
        /// Gets or sets Financial Section Verified.
        /// </summary>
        bool? FinancialSection { get; set; }

        /// <summary>
        /// Gets or sets signatureConfirmed.
        /// </summary>
        bool? SignatureConfirmed { get; set; }

        /// <summary>
        /// Gets or sets Taxpayer Name.
        /// </summary>
        string SignatureLine { get; set; }

        /// <summary>
        /// Gets or sets Taxpayer Signature Date.
        /// </summary>
        DateTime? SignatureDate { get; set; }

        /// <summary>
        /// Gets or sets Signature Section Verified.
        /// </summary>
        bool? SignatureSection { get; set; }

        /// <summary>
        /// Gets or sets Proof of Age - Applicant.
        /// </summary>
        bool? DocAgeApplicant { get; set; }

        /// <summary>
        /// Gets or sets Proof of Age - Spouse.
        /// </summary>
        bool? DocSpouseAge { get; set; }

        /// <summary>
        /// Gets or sets Proof of Disability.
        /// </summary>
        bool? DocDisability { get; set; }

        /// <summary>
        /// Gets or sets Proof of Ownership.
        /// </summary>
        bool? DocOwnership { get; set; }

        /// <summary>
        /// Gets or sets Proof of Major Life Change.
        /// </summary>
        bool? DocMajorLifechange { get; set; }

        /// <summary>
        /// Gets or sets Proof of Co-op Shares.
        /// </summary>
        bool? DocCoopShares { get; set; }

        /// <summary>
        /// Gets or sets a value for MissingDocumentList.
        /// </summary>
        string MissingDocumentList { get; set; }

        /// <summary>
        /// Gets or sets applicant Date of Birth.
        /// </summary>
        DateTime? DateofBirth { get; set; }

        /// <summary>
        /// Gets or sets applicant Email Address.
        /// </summary>
        string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets applicant First Name.
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// Gets or sets applicant Last Name.
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// Gets or sets applicant Middle Name.
        /// </summary>
        string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets applicant mobile phone.
        /// </summary>
        string MobilePhone { get; set; }

        /// <summary>
        /// Gets or sets applicant Phone Number.
        /// </summary>
        string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets applicant Suffix.
        /// </summary>
        string Suffix { get; set; }

        /// <summary>
        /// Gets or sets has Spouse Or Partner.
        /// </summary>
        bool? HasSpouseOrPartner { get; set; }

        /// <summary>
        /// Gets or sets residing For 9 Months.
        /// </summary>
        bool? ResidingFor9Months { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ownThroughTrust.
        /// </summary>
        bool? OwnThroughTrust { get; set; }

        /// <summary>
        /// Gets or sets otherCountyAddress.
        /// </summary>
        string OtherCountyAddress { get; set; }

        /// <summary>
        /// Gets or sets otherCountyCity.
        /// </summary>
        string OtherCountyCity { get; set; }

        /// <summary>
        /// Gets or sets otherCountyState.
        /// </summary>
        string OtherCountyState { get; set; }

        /// <summary>
        /// Gets or sets otherCountyPostal.
        /// </summary>
        string OtherCountyPostal { get; set; }

        /// <summary>
        /// Gets or sets ptas_checkaddresscity.
        /// </summary>
        string CheckAddressCity { get; set; }

        /// <summary>
        /// Gets or sets ptas_checkaddresscountryid (lookup).
        /// </summary>
        string CheckAddressCountryId { get; set; }

        /// <summary>
        /// Gets or sets ptas_checkaddressname.
        /// </summary>
        string CheckAddressName { get; set; }

        /// <summary>
        /// Gets or sets ptas_checkaddresspostalcode.
        /// </summary>
        string CheckAddressPostalCode { get; set; }

        /// <summary>
        /// Gets or sets ptas_checkaddressstate.
        /// </summary>
        string CheckAddressState { get; set; }

        /// <summary>
        /// Gets or sets ptas_checkaddressstreet.
        /// </summary>
        string CheckAddressStreet { get; set; }

        /// <summary>
        /// Gets or sets ptas_correspondencename.
        /// </summary>
        string CorrespondenceName { get; set; }

        /// <summary>
        /// Gets or sets ptas_differentcheckaddress.
        /// </summary>
        bool? DifferentCheckAddress { get; set; }

        /// <summary>
        /// Gets or sets ptas_otherparcelnumber.
        /// </summary>
        string OtherParcelNumber { get; set; }

        /// <summary>
        /// Gets or sets propertyTaxesDelinquent.
        /// </summary>
        bool? PropertyTaxesDelinquent { get; set; }

        /// <summary>
        /// Gets or sets collection of Prodefine Notes.
        /// </summary>
        ISEAppPredefNote[] SEApplicationPredefNotes { get; set; }

        /// <summary>
        /// Gets or sets the required to file federal income tax return flag.
        /// </summary>
        bool? RequiredToFileFederalIncomeTaxReturn { get; set; }

        /// <summary>
        /// Gets or sets a flags to identify as under 61 years old with disability notice.
        /// </summary>
        bool? Under61WithDisabilityNotice { get; set; }

        /// <summary>
        /// Gets or sets the veteran with service evaluation or disability flag.
        /// </summary>
        bool? VeteranWithServiceEvaluationOrDisability { get; set; }

        /// <summary>
        /// Gets or sets the is a surviving spouse flag.
        /// </summary>
        bool? IsASurvivingSpouse { get; set; }

        /// <summary>
        /// Gets or sets the married flag.
        /// </summary>
        bool? Married { get; set; }

        /// <summary>
        /// Gets or sets the single flag.
        /// </summary>
        bool? Single { get; set; }

        /// <summary>
        /// Gets or sets the widowed flag.
        /// </summary>
        bool? Widowed { get; set; }

        /// <summary>
        /// Gets or sets the divorced legally separed flag.
        /// </summary>
        bool? DivorcedLegallySeparated { get; set; }

        /// <summary>
        /// Gets or sets the married living apart flag.
        /// </summary>
        bool? MarriedLivingApart { get; set; }

        /// <summary>
        /// Gets or sets the property used for bussiness flag.
        /// </summary>
        bool? PropertyUsedForBusiness { get; set; }

        /// <summary>
        /// Gets or sets the percentage used for bussiness.
        /// </summary>
        double? PercentageUsedForBusiness { get; set; }

        /// <summary>
        /// Gets or sets the square footage used for the business.
        /// </summary>
        int? SquareFootageUsedForBusiness { get; set; }

        /// <summary>
        /// Gets or sets rent out a porcion of the property flag.
        /// </summary>
        bool? RentoutAPortionOfProperty { get; set; }

        /// <summary>
        /// Gets or sets percentage rented out value.
        /// </summary>
        double? PercentageRentedOut { get; set; }

        /// <summary>
        /// Gets or sets square footage rented out value.
        /// </summary>
        int? SquareFootageRentedOut { get; set; }

        /// <summary>
        /// Gets or sets the property purchased date.
        /// </summary>
        DateTime? DatePropertyPurchased { get; set; }

        /// <summary>
        /// Gets or sets the single family residence flag.
        /// </summary>
        bool? SingleFamilyResidence { get; set; }

        /// <summary>
        /// Gets or sets the housing co-op flag.
        /// </summary>
        bool? HousingCoOp { get; set; }

        /// <summary>
        /// Gets or sets the single unit of multi-dwelling (condo or duplex) flag.
        /// </summary>
        bool? SingleUnitOfMultiDwellingCondoOrDuplex { get; set; }

        /// <summary>
        /// Gets or sets the mobile home flag.
        /// </summary>
        bool? MobileHome { get; set; }

        /// <summary>
        /// Gets or sets the mobile home year.
        /// </summary>
        string MobileHomeYear { get; set; }

        /// <summary>
        /// Gets or sets the mobile home make value.
        /// </summary>
        string MobileHomeMake { get; set; }

        /// <summary>
        /// Gets or sets the mobile home model value.
        /// </summary>
        string MobileHomeModel { get; set; }

        /// <summary>
        /// Gets or sets the received exemption before? flag.
        /// </summary>
        bool? ReceivedExemptionBefore { get; set; }

        /// <summary>
        /// Gets or sets the date for the previous exemption value.
        /// </summary>
        DateTime? WhenWasThePreviousExemption { get; set; }

        /// <summary>
        /// Gets or sets the value for where the previous exemption was.
        /// </summary>
        string WhereWasThePreviousExemption { get; set; }

        /// <summary>
        /// Gets or sets the sold former residence? flag.
        /// </summary>
        bool? SoldFormerResidence { get; set; }

        /// <summary>
        /// Gets or sets the date of the former property sale value.
        /// </summary>
        DateTime? DateOfFormerPropertySale { get; set; }

        /// <summary>
        /// Gets or sets the sold other property in 2020? flag.
        /// </summary>
        bool? SoldOtherPropertyIn2020 { get; set; }

        /// <summary>
        /// Gets or sets the value for where the property was sold in 2020.
        /// </summary>
        string WherePropertyWasSoldIn2020 { get; set; }

        /// <summary>
        /// Gets or sets the date for when the prperty was sold in 2020 value.
        /// </summary>
        DateTime? DatePropertySoldIn2020 { get; set; }

        /// <summary>
        /// Gets or sets the date for the effective date of disability value.
        /// </summary>
        DateTime? EffectiveDateOfDisability { get; set; }
    }
}

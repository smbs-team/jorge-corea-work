// <copyright file="IParcelDetail.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    using System;

    /// <summary>
    /// Parcel Detail for crud.
    /// </summary>
    public interface IParcelDetail
    {
        /// <summary>
        /// Gets or sets a value for ParcelDetailId.
        /// ptas_parceldetailid.
        /// </summary>
        string ParcelDetailId { get; set; }

        /// <summary>
        ///  Gets or sets a value for CommArea.
        ///  ptas_commarea.
        /// </summary>
        int? CommArea { get; set; }

        /// <summary>
        ///  Gets or sets a value for DirSuffix.
        ///  ptas_dirsuffix.
        /// </summary>
        string DirSuffix { get; set; }

        /// <summary>
        /// Gets or sets a value for Modified By.
        /// _modifiedby_value.
        /// </summary>
        Guid? ModifiedBy { get; set; }

        /// <summary>
        ///  Gets or sets a value for BldgNbr.
        ///  ptas_bldgnbr.
        /// </summary>
        int? BldgNbr { get; set; }

        /// <summary>
        ///  Gets or sets a value for Acre.
        ///  ptas_acre.
        /// </summary>
        int? Acre { get; set; }

        /// <summary>
        ///  Gets or sets a value for PlatLot.
        ///  ptas_platlot.
        /// </summary>
        string PlatLot { get; set; }

        /// <summary>
        ///  Gets or sets a value for LegalDescription.
        ///  ptas_legaldescription.
        /// </summary>
        string LegalDescription { get; set; }

        /// <summary>
        /// Gets or sets a value for Created On Behalf By.
        /// _createdonbehalfby_value.
        /// </summary>
        Guid? CreatedOnBehalfBy { get; set; }

        /// <summary>
        ///  Gets or sets a value for MediaGuid.
        ///  ptas_mediaguid.
        /// </summary>
        string MediaGuid { get; set; }

        /// <summary>
        ///  Gets or sets a value for Zoning.
        ///  ptas_zoning.
        /// </summary>
        string Zoning { get; set; }

        /// <summary>
        /// Gets or sets a value for Created By.
        /// _createdby_value.
        /// </summary>
        Guid? CreatedBy { get; set; }

        /// <summary>
        ///  Gets or sets a value for ApplGroup.
        ///  ptas_applgroup.
        /// </summary>
        string ApplGroup { get; set; }

        /// <summary>
        ///  Gets or sets a value for AlternateKey.
        ///  ptas_alternatekey.
        /// </summary>
        string AlternateKey { get; set; }

        /// <summary>
        ///  Gets or sets a value for NamesOnAccount.
        ///  ptas_namesonaccount.
        /// </summary>
        string NamesOnAccount { get; set; }

        /// <summary>
        ///  Gets or sets a value for SplitCode.
        ///  ptas_splitcode.
        /// </summary>
        string SplitCode { get; set; }

        /// <summary>
        ///  Gets or sets a value for PropType.
        ///  ptas_proptype.
        /// </summary>
        string PropType { get; set; }

        /// <summary>
        ///  Gets or sets a value for TotalAccessoryValue.
        ///  ptas_totalaccessoryvalue.
        /// </summary>
        double? TotalAccessoryValue { get; set; }

        /// <summary>
        ///  Gets or sets a value for OtherExemptions.
        ///  ptas_otherexemptions.
        /// </summary>
        string OtherExemptions { get; set; }

        /// <summary>
        ///  Gets or sets a value for Minor.
        ///  ptas_minor.
        /// </summary>
        string Minor { get; set; }

        /// <summary>
        ///  Gets or sets a value for MediaType.
        ///  ptas_mediatype.
        /// </summary>
        int? MediaType { get; set; }

        /// <summary>
        ///  Gets or sets a value for PlatBlock.
        ///  ptas_platblock.
        /// </summary>
        string PlatBlock { get; set; }

        /// <summary>
        ///  Gets or sets a value for LandUseCode.
        ///  ptas_landusecode.
        /// </summary>
        string LandUseCode { get; set; }

        /// <summary>
        ///  Gets or sets a value for ChangeSource.
        ///  ptas_changesource.
        /// </summary>
        string ChangeSource { get; set; }

        /// <summary>
        ///  Gets or sets a value for TotalAccessoryValueBase.
        ///  ptas_totalaccessoryvalue_base.
        /// </summary>
        double? TotalAccessoryValueBase { get; set; }

        /// <summary>
        ///  Gets or sets a value for NbrLivingUnits.
        ///  ptas_nbrlivingunits.
        /// </summary>
        int? NbrLivingUnits { get; set; }

        /// <summary>
        ///  Gets or sets a value for Neighborhood.
        ///  ptas_neighborhood.
        /// </summary>
        int? Neighborhood { get; set; }

        /// <summary>
        ///  Gets or sets a value for StreetName.
        ///  ptas_streetname.
        /// </summary>
        string StreetName { get; set; }

        /// <summary>
        ///  Gets or sets a value for Major.
        ///  ptas_major.
        /// </summary>
        string Major { get; set; }

        /// <summary>
        ///  Gets or sets a value for CommSubarea.
        ///  ptas_commsubarea.
        /// </summary>
        int? CommSubarea { get; set; }

        /// <summary>
        ///  Gets or sets a value for Folio.
        ///  ptas_folio.
        /// </summary>
        string Folio { get; set; }

        /// <summary>
        ///  Gets or sets a value for StreetNbr.
        ///  ptas_streetnbr.
        /// </summary>
        string StreetNbr { get; set; }

        /// <summary>
        ///  Gets or sets a value for LevyCode.
        ///  ptas_levycode.
        /// </summary>
        string LevyCode { get; set; }

        /// <summary>
        ///  Gets or sets a value for StreetType.
        ///  ptas_streettype.
        /// </summary>
        string StreetType { get; set; }

        /// <summary>
        ///  Gets or sets a value for NumberOfBuildings.
        ///  ptas_numberofbuildings.
        /// </summary>
        int? NumberOfBuildings { get; set; }

        /// <summary>
        ///  Gets or sets a value for AcctNbr.
        ///  ptas_acctnbr.
        /// </summary>
        string AcctNbr { get; set; }

        /// <summary>
        ///  Gets or sets a value for Name.
        ///  ptas_name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///  Gets or sets a value for ResArea.
        ///  ptas_resarea.
        /// </summary>
        int? ResArea { get; set; }

        /// <summary>
        ///  Gets or sets a value for Address.
        ///  ptas_address.
        /// </summary>
        string Address { get; set; }

        /// <summary>
        ///  Gets or sets a value for SqftLot.
        ///  ptas_sqftlot.
        /// </summary>
        int? SqftLot { get; set; }

        /// <summary>
        ///  Gets or sets a value for RpaAlternateKey.
        ///  ptas_rpaalternatekey.
        /// </summary>
        string RpaAlternateKey { get; set; }

        /// <summary>
        ///  Gets or sets a value for LandUseDesc.
        ///  ptas_landusedesc.
        /// </summary>
        string LandUseDesc { get; set; }

        /// <summary>
        ///  Gets or sets a value for District.
        ///  ptas_district.
        /// </summary>
        string District { get; set; }

        /// <summary>
        ///  Gets or sets a value for ZipCode.
        ///  ptas_zipcode.
        /// </summary>
        string ZipCode { get; set; }

        /// <summary>
        ///  Gets or sets a value for ResSubarea.
        ///  ptas_ressubarea.
        /// </summary>
        int? ResSubarea { get; set; }

        /// <summary>
        ///  Gets or sets a value for NewConstrVal.
        ///  ptas_newconstrval.
        /// </summary>
        string NewConstrVal { get; set; }

        /// <summary>
        ///  Gets or sets a value for NbrFraction.
        ///  ptas_nbrfraction.
        /// </summary>
        string NbrFraction { get; set; }

        /// <summary>
        ///  Gets or sets a value for LandAlternateKey.
        ///  ptas_landalternatekey.
        /// </summary>
        string LandAlternateKey { get; set; }
    }
}
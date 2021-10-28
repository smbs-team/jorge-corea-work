// <copyright file="DynamicsParcelDetail.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;
    using Newtonsoft.Json;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Parcel Detail info returned by dynamics.
    /// </summary>
    public class DynamicsParcelDetail : IParcelDetail
    {
        /// <inheritdoc/>
        [JsonProperty("ptas_parceldetailid")]
        public string ParcelDetailId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_commarea")]
        public int? CommArea { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_dirsuffix")]
        public string DirSuffix { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_modifiedby_value", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? ModifiedBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_bldgnbr")]
        public int? BldgNbr { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_acre")]
        public int? Acre { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_platlot")]
        public string PlatLot { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_legaldescription")]
        public string LegalDescription { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_createdonbehalfby_value", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? CreatedOnBehalfBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_mediaguid")]
        public string MediaGuid { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_zoning")]
        public string Zoning { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_createdby_value", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? CreatedBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_applgroup")]
        public string ApplGroup { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_alternatekey")]
        public string AlternateKey { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_namesonaccount")]
        public string NamesOnAccount { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_splitcode")]
        public string SplitCode { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_proptype")]
        public string PropType { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_totalaccessoryvalue")]
        public double? TotalAccessoryValue { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_otherexemptions")]
        public string OtherExemptions { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_minor")]
        public string Minor { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_mediatype")]
        public int? MediaType { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_platblock")]
        public string PlatBlock { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_landusecode")]
        public string LandUseCode { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_changesource")]
        public string ChangeSource { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_totalaccessoryvalue_base")]
        public double? TotalAccessoryValueBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_nbrlivingunits")]
        public int? NbrLivingUnits { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_neighborhood")]
        public int? Neighborhood { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_streetname")]
        public string StreetName { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_major")]
        public string Major { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_commsubarea")]
        public int? CommSubarea { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_folio")]
        public string Folio { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_streetnbr")]
        public string StreetNbr { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_levycode")]
        public string LevyCode { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_streettype")]
        public string StreetType { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_numberofbuildings")]
        public int? NumberOfBuildings { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_acctnbr")]
        public string AcctNbr { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_name")]
        public string Name { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_resarea")]
        public int? ResArea { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_address")]
        public string Address { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_sqftlot")]
        public int? SqftLot { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_rpaalternatekey")]
        public string RpaAlternateKey { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_landusedesc")]
        public string LandUseDesc { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_district")]
        public string District { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_zipcode")]
        public string ZipCode { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_ressubarea")]
        public int? ResSubarea { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_newconstrval")]
        public string NewConstrVal { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_nbrfraction")]
        public string NbrFraction { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_landalternatekey")]
        public string LandAlternateKey { get; set; }
    }
}

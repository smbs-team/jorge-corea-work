// <copyright file="FormParcelDetail.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
  using System;
  using Newtonsoft.Json;
  using PTASDynamicsCrudHelperClasses.Classes;
  using PTASDynamicsCrudHelperClasses.Interfaces;

  /// <summary>
  /// Parcel Detail to be read from API.
  /// </summary>
  public class FormParcelDetail : FormInput, IParcelDetail
  {
    /// <inheritdoc/>
    [JsonProperty("parceldetailid")]
    public string ParcelDetailId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("commarea")]
    public int? CommArea { get; set; }

    /// <inheritdoc/>
    [JsonProperty("dirsuffix")]
    public string DirSuffix { get; set; }

    /// <inheritdoc/>
    [JsonProperty("_modifiedby_value", NullValueHandling = NullValueHandling.Ignore)]
    public Guid? ModifiedBy { get; set; }

    /// <inheritdoc/>
    [JsonProperty("bldgnbr")]
    public int? BldgNbr { get; set; }

    /// <inheritdoc/>
    [JsonProperty("acre")]
    public int? Acre { get; set; }

    /// <inheritdoc/>
    [JsonProperty("platlot")]
    public string PlatLot { get; set; }

    /// <inheritdoc/>
    [JsonProperty("legaldescription")]
    public string LegalDescription { get; set; }

    /// <inheritdoc/>
    [JsonProperty("_createdonbehalfby_value", NullValueHandling = NullValueHandling.Ignore)]
    public Guid? CreatedOnBehalfBy { get; set; }

    /// <inheritdoc/>
    [JsonProperty("mediaguid")]
    public string MediaGuid { get; set; }

    /// <inheritdoc/>
    [JsonProperty("zoning")]
    public string Zoning { get; set; }

    /// <inheritdoc/>
    [JsonProperty("_createdby_value", NullValueHandling = NullValueHandling.Ignore)]
    public Guid? CreatedBy { get; set; }

    /// <inheritdoc/>
    [JsonProperty("applgroup")]
    public string ApplGroup { get; set; }

    /// <inheritdoc/>
    [JsonProperty("alternatekey")]
    public string AlternateKey { get; set; }

    /// <inheritdoc/>
    [JsonProperty("namesonaccount")]
    public string NamesOnAccount { get; set; }

    /// <inheritdoc/>
    [JsonProperty("splitcode")]
    public string SplitCode { get; set; }

    /// <inheritdoc/>
    [JsonProperty("proptype")]
    public string PropType { get; set; }

    /// <inheritdoc/>
    [JsonProperty("totalaccessoryvalue")]
    public double? TotalAccessoryValue { get; set; }

    /// <inheritdoc/>
    [JsonProperty("otherexemptions")]
    public string OtherExemptions { get; set; }

    /// <inheritdoc/>
    [JsonProperty("minor")]
    public string Minor { get; set; }

    /// <inheritdoc/>
    [JsonProperty("mediatype")]
    public int? MediaType { get; set; }

    /// <inheritdoc/>
    [JsonProperty("platblock")]
    public string PlatBlock { get; set; }

    /// <inheritdoc/>
    [JsonProperty("landusecode")]
    public string LandUseCode { get; set; }

    /// <inheritdoc/>
    [JsonProperty("changesource")]
    public string ChangeSource { get; set; }

    /// <inheritdoc/>
    [JsonProperty("totalaccessoryvalue_base")]
    public double? TotalAccessoryValueBase { get; set; }

    /// <inheritdoc/>
    [JsonProperty("nbrlivingunits")]
    public int? NbrLivingUnits { get; set; }

    /// <inheritdoc/>
    [JsonProperty("neighborhood")]
    public int? Neighborhood { get; set; }

    /// <inheritdoc/>
    [JsonProperty("streetname")]
    public string StreetName { get; set; }

    /// <inheritdoc/>
    [JsonProperty("major")]
    public string Major { get; set; }

    /// <inheritdoc/>
    [JsonProperty("commsubarea")]
    public int? CommSubarea { get; set; }

    /// <inheritdoc/>
    [JsonProperty("folio")]
    public string Folio { get; set; }

    /// <inheritdoc/>
    [JsonProperty("streetnbr")]
    public string StreetNbr { get; set; }

    /// <inheritdoc/>
    [JsonProperty("levycode")]
    public string LevyCode { get; set; }

    /// <inheritdoc/>
    [JsonProperty("streettype")]
    public string StreetType { get; set; }

    /// <inheritdoc/>
    [JsonProperty("numberofbuildings")]
    public int? NumberOfBuildings { get; set; }

    /// <inheritdoc/>
    [JsonProperty("acctnbr")]
    public string AcctNbr { get; set; }

    /// <inheritdoc/>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <inheritdoc/>
    [JsonProperty("resarea")]
    public int? ResArea { get; set; }

    /// <inheritdoc/>
    [JsonProperty("address")]
    public string Address { get; set; }

    /// <inheritdoc/>
    [JsonProperty("sqftlot")]
    public int? SqftLot { get; set; }

    /// <inheritdoc/>
    [JsonProperty("rpaalternatekey")]
    public string RpaAlternateKey { get; set; }

    /// <inheritdoc/>
    [JsonProperty("landusedesc")]
    public string LandUseDesc { get; set; }

    /// <inheritdoc/>
    [JsonProperty("district")]
    public string District { get; set; }

    /// <inheritdoc/>
    [JsonProperty("zipcode")]
    public string ZipCode { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ressubarea")]
    public int? ResSubarea { get; set; }

    /// <inheritdoc/>
    [JsonProperty("newconstrval")]
    public string NewConstrVal { get; set; }

    /// <inheritdoc/>
    [JsonProperty("nbrfraction")]
    public string NbrFraction { get; set; }

    /// <inheritdoc/>
    [JsonProperty("landalternatekey")]
    public string LandAlternateKey { get; set; }

    /// <summary>
    /// Sets the id for a new object.
    /// </summary>
    public override void SetId()
    {
      if (string.IsNullOrEmpty(this.ParcelDetailId))
      {
        this.ParcelDetailId = Guid.NewGuid().ToString();
      }
    }
  }
}

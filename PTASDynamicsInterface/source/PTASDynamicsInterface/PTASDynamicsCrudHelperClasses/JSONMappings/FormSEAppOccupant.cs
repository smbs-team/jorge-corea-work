// <copyright file="FormSEAppOccupant.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
  using System;
  using Newtonsoft.Json;
  using PTASDynamicsCrudHelperClasses.Classes;
  using PTASDynamicsCrudHelperClasses.Interfaces;

  /// <summary>
  /// SEAppOccupant to be read from API.
  /// </summary>
  public class FormSEAppOccupant : FormInput, ISEAppOccupant
  {
    /// <inheritdoc/>
    [JsonProperty("seappoccupantId")]
    public string SEAppOccupantId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("seapplicationid")]
    public string SEApplicationId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("occupanttype")]
    public int? OccupantType { get; set; }

    /// <inheritdoc/>
    [JsonProperty("occupantlastname")]
    public string OccupantLastName { get; set; }

    /// <inheritdoc/>
    [JsonProperty("occupantfirstName")]
    public string OccupantFirstName { get; set; }

    /// <inheritdoc/>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <inheritdoc/>
    [JsonProperty("occupantmiddlename")]
    public string OccupantMiddleName { get; set; }

    /// <inheritdoc/>
    [JsonProperty("occupantsuffix")]
    public string OccupantSuffix { get; set; }

    /// <inheritdoc/>
    [JsonProperty("createdon")]
    public DateTime? CreatedOn { get; set; }

    /// <inheritdoc/>
    [JsonProperty("createdby")]
    public Guid? CreatedBy { get; set; }

    /// <inheritdoc/>
    [JsonProperty("modifiedon")]
    public DateTime? ModifiedOn { get; set; }

    /// <inheritdoc/>
    [JsonProperty("modifiedby")]
    public Guid? ModifiedBy { get; set; }

    /// <summary>
    /// Sets the id for a new object.
    /// </summary>
    public override void SetId()
    {
      if (string.IsNullOrEmpty(this.SEAppOccupantId))
      {
        this.SEAppOccupantId = Guid.NewGuid().ToString();
      }
    }
  }
}

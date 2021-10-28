// <copyright file="FormSEAppNote.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
  using System;
  using Newtonsoft.Json;
  using PTASDynamicsCrudHelperClasses.Classes;
  using PTASDynamicsCrudHelperClasses.Interfaces;

  /// <summary>
  /// Senior Exemption Application Note to be read from API.
  /// </summary>
  public class FormSEAppNote : FormInput, ISEAppNote
  {
    /// <inheritdoc/>
    [JsonProperty("ptas_seappnoteid")]
    public string SEAppNoteId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_seapplicationid")]
    public string SEApplicationId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("statecode")]
    public int? StateCode { get; set; }

    /// <inheritdoc/>
    [JsonProperty("statuscode")]
    public int? StatusCode { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_description")]
    public string Description { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_name")]
    public string Name { get; set; }

    /// <inheritdoc/>
    [JsonProperty("_ptas_seappdetailid_value")]
    public string SEAppDetailId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ptas_showonportal")]
    public bool? ShowOnPortal { get; set; }

    /// <inheritdoc/>
    [JsonProperty("createdby")]
    public Guid? CreatedBy { get; set; }

    /// <inheritdoc/>
    [JsonProperty("createdon")]
    public DateTime? CreatedOn { get; set; }

    /// <inheritdoc/>
    [JsonProperty("modifiedby")]
    public Guid? ModifiedBy { get; set; }

    /// <inheritdoc/>
    [JsonProperty("modifiedon")]
    public DateTime? ModifiedOn { get; set; }

    /// <summary>
    /// Sets the id for a new object.
    /// </summary>
    public override void SetId()
    {
      if (string.IsNullOrEmpty(this.SEAppNoteId))
      {
        this.SEAppNoteId = Guid.NewGuid().ToString();
      }
    }
  }
}
// <copyright file="FormContact.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using Newtonsoft.Json;
  using PTASDynamicsCrudHelperClasses.Classes;
  using PTASDynamicsCrudHelperClasses.Interfaces;

  /// <summary>
  /// Contact to be read from API.
  /// </summary>
  public class FormContact : FormInput, IContact
  {
    /// <inheritdoc/>
    [JsonProperty("contactid")]
    public string ContactId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("firstname")]
    public string FirstName { get; set; }

    /// <inheritdoc/>
    [JsonProperty("lastname")]
    public string LastName { get; set; }

    /// <inheritdoc/>
    [JsonProperty("emailaddress")]
    [EmailAddress]
    [Required]
    public string EmailAddress { get; set; }

    /// <inheritdoc/>
    [JsonProperty("middlename")]
    public string MiddleName { get; set; }

    /// <inheritdoc/>
    [JsonProperty("phone")]
    [Phone]
    public string Phone { get; set; }

    /// <inheritdoc/>
    [JsonProperty("smscapable")]
    public bool? SMSCapable { get; set; } = false;

    /// <inheritdoc/>
    [JsonProperty("alternatekey")]
    public int? AlternateKey { get; set; }

    /// <inheritdoc/>
    [JsonProperty("suffix")]
    public string Suffix { get; set; }

    /// <inheritdoc/>
    [JsonProperty("birthdate")]
    public System.DateTime? BirthDate { get; set; }

    /// <summary>
    /// Sets the id for a new contact.
    /// </summary>
    public override void SetId()
    {
      if (string.IsNullOrEmpty(this.ContactId))
      {
        this.ContactId = Guid.NewGuid().ToString();
      }
    }
  }
}

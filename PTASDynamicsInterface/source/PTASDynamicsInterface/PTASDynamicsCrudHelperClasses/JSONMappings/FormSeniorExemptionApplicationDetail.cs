// <copyright file="FormSeniorExemptionApplicationDetail.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
  using System;
  using Newtonsoft.Json;
  using PTASDynamicsCrudHelperClasses.Classes;
  using PTASDynamicsCrudHelperClasses.Interfaces;

  /// <summary>
  /// Senior Exemption Application Detail to be read from API.
  /// </summary>
  public class FormSeniorExemptionApplicationDetail : FormInput, ISeniorExemptionApplicationDetail
  {
    /// <inheritdoc/>
    [JsonProperty("seappdetailid")]
    public string SEAppdetailid { get; set; }

    /// <inheritdoc/>
    [JsonProperty("ageondecember31")]
    public int? AgeOnDecember31 { get; set; }

    /// <inheritdoc/>
    [JsonProperty("decisionreasonid")]
    public string DecisionReasonId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("docannuity")]
    public bool? DocAnnuity { get; set; }

    /// <inheritdoc/>
    [JsonProperty("docbankstatement")]
    public bool? DocBankStatement { get; set; }

    /// <inheritdoc/>
    [JsonProperty("docira")]
    public bool? DocIRA { get; set; }

    /// <inheritdoc/>
    [JsonProperty("docirs1040")]
    public bool? DocIRS1040 { get; set; }

    /// <inheritdoc/>
    [JsonProperty("docirs1099")]
    public bool? DocIRS1099 { get; set; }

    /// <inheritdoc/>
    [JsonProperty("docother")]
    public string DocOther { get; set; }

    /// <inheritdoc/>
    [JsonProperty("docpension")]
    public bool? DocPension { get; set; }

    /// <inheritdoc/>
    [JsonProperty("docrx")]
    public bool? DocRx { get; set; }

    /// <inheritdoc/>
    [JsonProperty("docsocialsecurity")]
    public bool? DocSocialSecurity { get; set; }

    /// <inheritdoc/>
    [JsonProperty("exemptiontype")]
    public int? ExemptionType { get; set; }

    /// <inheritdoc/>
    [JsonProperty("incomelevel")]
    public int? IncomeLevel { get; set; }

    /// <inheritdoc/>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <inheritdoc/>
    [JsonProperty("otherreason")]
    public string OtherReason { get; set; }

    /// <inheritdoc/>
    [JsonProperty("parcelid", NullValueHandling = NullValueHandling.Ignore)]
    public string ParcelId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("seapplicationid", NullValueHandling = NullValueHandling.Ignore)]
    public string SeApplicationId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("yearid", NullValueHandling = NullValueHandling.Ignore)]
    public string YearId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("totalincome")]
    public double? TotalIncome { get; set; }

    /// <inheritdoc/>
    [JsonProperty("totalincomebase")]
    public double? TotalIncomeBase { get; set; }

    /// <inheritdoc/>
    [JsonProperty("totalexpenses")]
    public double? TotalExpenses { get; set; }

    /// <inheritdoc/>
    [JsonProperty("totalexpensesbase")]
    public double? TotalExpensesBase { get; set; }

    /// <inheritdoc/>
    [JsonProperty("nettotalincome")]
    public double? NetTotalIncome { get; set; }

    /// <inheritdoc/>
    [JsonProperty("nettotalincomebase")]
    public double? NetTotalIncomeBase { get; set; }

    /// <inheritdoc/>
    [JsonProperty("missingdocumentlist")]
    public string MissingDocumentList { get; set; }

    /// <inheritdoc/>
    [JsonProperty("alternatekey")]
    public int? AlternateKey { get; set; }

    /// <inheritdoc/>
    [JsonProperty("accountnumber")]
    public string AccountNumber { get; set; }

    /// <inheritdoc/>
    [JsonProperty("contactid", NullValueHandling = NullValueHandling.Ignore)]
    public string ContactId { get; set; }

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
    [JsonProperty("statuscode")]
    public int? StatusCode { get; set; }

    /// <inheritdoc/>
    [JsonProperty("statecode")]
    public int? StateCode { get; set; }

    /// <summary>
    /// Sets the id for a new entity.
    /// </summary>
    public override void SetId()
    {
      if (string.IsNullOrEmpty(this.SEAppdetailid))
      {
        this.SEAppdetailid = Guid.NewGuid().ToString();
      }
    }
  }
}

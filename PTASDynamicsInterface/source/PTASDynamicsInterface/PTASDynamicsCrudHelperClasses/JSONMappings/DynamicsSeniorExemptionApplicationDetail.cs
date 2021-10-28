﻿// <copyright file="DynamicsSeniorExemptionApplicationDetail.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;
    using System.ComponentModel;
    using Newtonsoft.Json;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Senior Exemption Application Detail info returned by dynamics.
    /// </summary>
    public class DynamicsSeniorExemptionApplicationDetail : ISeniorExemptionApplicationDetail
    {
        /// <inheritdoc/>
        [JsonProperty("ptas_seappdetailid")]
        public string SEAppdetailid { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_ageasofdecember31")]
        public int? AgeOnDecember31 { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_ptas_decisionreasonid_value", NullValueHandling = NullValueHandling.Ignore)]
        public string DecisionReasonId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docannuity")]
        public bool? DocAnnuity { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docbankstatement")]
        public bool? DocBankStatement { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docira")]
        public bool? DocIRA { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docirs1040")]
        public bool? DocIRS1040 { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docirs1099")]
        public bool? DocIRS1099 { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docother")]
        public string DocOther { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docpension")]
        public bool? DocPension { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docrx")]
        public bool? DocRx { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_docsocialsecurity")]
        public bool? DocSocialSecurity { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_exemptiontype")]
        public int? ExemptionType { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomelevel")]
        public int? IncomeLevel { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_name")]
        public string Name { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_other")]
        public string OtherReason { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_ptas_parcelid_value", NullValueHandling = NullValueHandling.Ignore)]
        public string ParcelId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_ptas_seapplicationid_value", NullValueHandling = NullValueHandling.Ignore)]
        public string SeApplicationId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_ptas_yearid_value", NullValueHandling = NullValueHandling.Ignore)]
        public string YearId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_totalincome")]
        public double? TotalIncome { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_totalincome_base")]
        public double? TotalIncomeBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_totalexpenses")]
        public double? TotalExpenses { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_totalexpenses_base")]
        public double? TotalExpensesBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_nettotalincome")]
        public double? NetTotalIncome { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_nettotalincome_base")]
        public double? NetTotalIncomeBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_missingdocumentlist")]
        public string MissingDocumentList { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_alternatekey")]
        public int? AlternateKey { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_accountnumber")]
        public string AccountNumber { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_ptas_contactid_value", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactId { get; set; }

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
        [JsonProperty("statuscode")]
        public int? StatusCode { get; set; }

        /// <inheritdoc/>
        [DefaultValue(0)]
        [JsonProperty("statecode", DefaultValueHandling = DefaultValueHandling.Populate)]
        public int? StateCode { get; set; } = 0;
    }
}

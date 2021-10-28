// <copyright file="ISeniorExemptionApplicationDetail.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    using System;

    /// <summary>
    /// Senior Exemption Application Detail for crud.
    /// </summary>
    public interface ISeniorExemptionApplicationDetail
    {
        /// <summary>
        /// Gets or sets a value for SEAppdetailid.
        /// </summary>
        string SEAppdetailid { get; set; }

        /// <summary>
        ///  Gets or sets the age on december 31.
        /// </summary>
        int? AgeOnDecember31 { get; set; }

        /// <summary>
        /// Gets or sets a value for DecisionReasonId.
        /// </summary>
        string DecisionReasonId { get; set; }

        /// <summary>
        ///  Gets or sets a value for DocAnnuity.
        /// </summary>
        bool? DocAnnuity { get; set; }

        /// <summary>
        ///  Gets or sets a value for DocBankStatement.
        /// </summary>
        bool? DocBankStatement { get; set; }

        /// <summary>
        ///  Gets or sets a value for DocIRA.
        /// </summary>
        bool? DocIRA { get; set; }

        /// <summary>
        ///  Gets or sets a value if doc is DocIRS1040.
        /// </summary>
        bool? DocIRS1040 { get; set; }

        /// <summary>
        ///  Gets or sets a value if doc is IRS1099.
        /// </summary>
        bool? DocIRS1099 { get; set; }

        /// <summary>
        ///  Gets or sets a value for DocOther.
        /// </summary>
        string DocOther { get; set; }

        /// <summary>
        ///  Gets or sets a value for DocPension.
        /// </summary>
        bool? DocPension { get; set; }

        /// <summary>
        ///  Gets or sets a value for DocRx.
        /// </summary>
        bool? DocRx { get; set; }

        /// <summary>
        ///  Gets or sets a value for DocSocialSecurity.
        /// </summary>
        bool? DocSocialSecurity { get; set; }

        /// <summary>
        ///  Gets or sets a value for ExemptionType.
        /// </summary>
        int? ExemptionType { get; set; }

        /// <summary>
        ///  Gets or sets a value for IncomeLevel.
        /// </summary>
        int? IncomeLevel { get; set; }

        /// <summary>
        ///  Gets or sets a value for Name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///  Gets or sets a value for OtherReason.
        /// </summary>
        string OtherReason { get; set; }

        /// <summary>
        /// Gets or sets a value for ParcelId.
        /// </summary>
        string ParcelId { get; set; }

        /// <summary>
        /// Gets or sets a value for SeApplicationId.
        /// </summary>
        string SeApplicationId { get; set; }

        /// <summary>
        /// Gets or sets a value for TotalIncome.
        /// </summary>
        double? TotalIncome { get; set; }

        /// <summary>
        /// Gets or sets a value for TotalIncomeBase.
        /// </summary>
        double? TotalIncomeBase { get; set; }

        /// <summary>
        /// Gets or sets a value for TotalExpenses.
        /// </summary>
        double? TotalExpenses { get; set; }

        /// <summary>
        /// Gets or sets a value for TotalExpenses.
        /// </summary>
        double? TotalExpensesBase { get; set; }

        /// <summary>
        /// Gets or sets a value for NetTotalIncome.
        /// </summary>
        double? NetTotalIncome { get; set; }

        /// <summary>
        /// Gets or sets a value for NetTotalIncomeBase.
        /// </summary>
        double? NetTotalIncomeBase { get; set; }

        /// <summary>
        /// Gets or sets a value for MissingDocumentList.
        /// </summary>
        string MissingDocumentList { get; set; }

        /// <summary>
        ///  Gets or sets a value for AlternateKey.
        /// </summary>
        int? AlternateKey { get; set; }

        /// <summary>
        /// Gets or sets a value for AccountNumber.
        /// </summary>
        string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets a value for SeApplicationId.
        /// </summary>
        string ContactId { get; set; }

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
        /// Gets or sets a value for YearId.
        /// _ptas_yearid_value.
        /// </summary>
        string YearId { get; set; }

        /// <summary>
        /// Gets or sets a value for statuscode.
        /// statuscode.
        /// </summary>
        int? StatusCode { get; set; }

        /// <summary>
        ///  Gets or sets a value for State Code.
        /// </summary>
        int? StateCode { get; set; }
    }
}

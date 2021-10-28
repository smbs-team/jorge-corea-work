// <copyright file="ISeniorExemptionApplicationFinancial.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    using System;

    /// <summary>
    /// Senior Exemption Application Financial for crud.
    /// </summary>
    public interface ISeniorExemptionApplicationFinancial
    {
        /// <summary>
        /// Gets or sets a value for SEAppdetailid.
        /// ptas_sefinancialformsid.
        /// </summary>
        string SEFinancialFormsId { get; set; }

        /// <summary>
        ///  Gets or sets a value for AlternateKey.
        ///  ptas_alternatekey.
        /// </summary>
        int? AlternateKey { get; set; }

        /// <summary>
        /// Gets or sets a value for SEAppdetailid.
        /// _ptas_seappdetailid_value.
        /// </summary>
        string SEAppDetailId { get; set; }

        /// <summary>
        /// Gets or sets a value for YearId.
        /// _ptas_yearid_value.
        /// </summary>
        string YearId { get; set; }

        /// <summary>
        /// Gets or sets a value for FinancialFormType.
        /// ptas_financialformtype.
        /// </summary>
        int? FinancialFormType { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040EXWagesBase.
        /// ptas_1040ezwages_base.
        /// </summary>
        double? F1040EXWagesBase { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1FarmIncomeLossBase.
        /// ptas_schd1farmincomeloss_base.
        /// </summary>
        double? Schd1FarmIncomeLossBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeWages.
        /// ptas_incomewages.
        /// </summary>
        double? IncomeWages { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddCapgains.
        /// ptas_schddcapgains.
        /// </summary>
        double? SchddCapgains { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017RoyalTiesTrustSetc.
        /// ptas_10402017royaltiestrustsetc.
        /// </summary>
        double? F10402017RoyalTiesTrustSetc { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aTaxRefunds.
        /// ptas_1040ataxrefunds.
        /// </summary>
        double? F1040aTaxRefunds { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018TaxInterest.
        /// ptas_10402018taxinterest.
        /// </summary>
        double? F10402018TaxInterest { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017CapGainLoss.
        /// ptas_10402017capgainloss.
        /// </summary>
        double? F10402017CapGainLoss { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeServiceDisabilityBase.
        /// ptas_incomeservicedisability_base.
        /// </summary>
        double? IncomeServiceDisabilityBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aPensionsBase.
        /// ptas_1040apensions_base.
        /// </summary>
        double? F1040aPensionsBase { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1UnemploymentBase.
        /// ptas_schd1unemployment_base.
        /// </summary>
        double? Schd1UnemploymentBase { get; set; }

        /// <summary>
        /// Gets or sets a value for Created On Behalf By.
        /// _modifiedonbehalfby_value.
        /// </summary>
        Guid? ModifiedOnBehalfBy { get; set; }

        /// <summary>
        /// Gets or sets a value for F8949totalgains_base.
        /// ptas_8949totalgains_base.
        /// </summary>
        double? F8949totalgains_base { get; set; }

        /// <summary>
        /// Gets or sets a value for SchdCnetProfitLossBase.
        /// ptas_schdcnetprofitloss_base.
        /// </summary>
        double? SchdCnetProfitLossBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeExpensesBase.
        /// ptas_incomeexpenses_base.
        /// </summary>
        double? IncomeExpensesBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040eztaxinterest.
        /// ptas_1040eztaxinterest.
        /// </summary>
        double? F1040eztaxinterest { get; set; }

        /// <summary>
        /// Gets or sets a value for Created On Behalf By.
        /// _createdonbehalfby_value.
        /// </summary>
        Guid? CreatedOnBehalfBy { get; set; }

        /// <summary>
        /// Gets or sets a value for Created By.
        /// _createdby_value.
        /// </summary>
        Guid? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018Dividends.
        /// ptas_10402018dividends.
        /// </summary>
        double? F10402018Dividends { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeGamblingBase.
        /// ptas_incomegambling_base.
        /// </summary>
        double? IncomeGamblingBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeUnemploymentBase.
        /// ptas_incomeunemployment_base.
        /// </summary>
        double? IncomeUnemploymentBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeGiftsBase.
        /// ptas_incomegifts_base.
        /// </summary>
        double? IncomeGiftsBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018DividendsBase.
        /// ptas_10402018dividends_base.
        /// </summary>
        double? F10402018DividendsBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aordDividendsBase.
        /// ptas_1040aorddividends_base.
        /// </summary>
        double? F1040aordDividendsBase { get; set; }

        /// <summary>
        /// Gets or sets a value for SchdfDepreciation.
        /// ptas_schdfdepreciation.
        /// </summary>
        double? SchdfDepreciation { get; set; }

        /// <summary>
        /// Gets or sets a value for F8829PercentUsedForBusiness.
        /// ptas_8829percentusedforbusiness.
        /// </summary>
        int? F8829PercentUsedForBusiness { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018TaxEXInterest.
        /// ptas_10402018taxexinterest.
        /// </summary>
        double? F10402018TaxEXInterest { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddLongTermBoxb.
        /// ptas_schddlongtermboxb.
        /// </summary>
        double? SchddLongTermBoxb { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040EZWages.
        /// ptas_1040ezwages.
        /// </summary>
        double? F1040EZWages { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeDividends.
        /// ptas_incomedividends.
        /// </summary>
        double? IncomeDividends { get; set; }

        /// <summary>
        /// Gets or sets a value for ExpenseOtherBase.
        /// ptas_expenseother_base.
        /// </summary>
        double? ExpenseOtherBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018TotalIncomeBase.
        /// ptas_10402018totalincome_base.
        /// </summary>
        double? F10402018TotalIncomeBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017OtherIncome.
        /// ptas_10402017otherincome.
        /// </summary>
        double? F10402017OtherIncome { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeCountriesBase.
        /// ptas_incomecountries_base.
        /// </summary>
        double? IncomeCountriesBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeIraTaxAmt.
        /// ptas_incomeirataxamt.
        /// </summary>
        double? IncomeIraTaxAmt { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018adjgrossincome.
        /// ptas_10402018adjgrossincome.
        /// </summary>
        double? F10402018adjgrossincome { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1Unemployment.
        /// ptas_schd1unemployment.
        /// </summary>
        double? Schd1Unemployment { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1CapGainLoss.
        /// ptas_schd1capgainloss.
        /// </summary>
        double? Schd1CapGainLoss { get; set; }

        /// <summary>
        /// Gets or sets a value for ExpenseMedicarePlan.
        /// ptas_expensemedicareplan.
        /// </summary>
        double? ExpenseMedicarePlan { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddLongTermBoxcBase.
        /// ptas_schddlongtermboxc_base.
        /// </summary>
        double? SchddLongTermBoxcBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aDeductionsBase.
        /// ptas_1040adeductions_base.
        /// </summary>
        double? F1040aDeductionsBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017Dividends.
        /// ptas_10402017dividends.
        /// </summary>
        double? F10402017Dividends { get; set; }

        /// <summary>
        /// Gets or sets a value for ExpenseNursingPayee.
        /// ptas_expensenursingpayee.
        /// </summary>
        string ExpenseNursingPayee { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddShortTerm6252Base.
        /// ptas_schddshortterm6252_base.
        /// </summary>
        double? SchddShortTerm6252Base { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeBusinessBase.
        /// ptas_incomebusiness_base.
        /// </summary>
        double? IncomeBusinessBase { get; set; }

        /// <summary>
        /// Gets or sets a value for SchdcOtherIncomeAmtBase.
        /// ptas_schdcotherincomeamt_base.
        /// </summary>
        double? SchdcOtherIncomeAmtBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeDisability.
        /// ptas_incomedisability.
        /// </summary>
        double? IncomeDisability { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeRetirementBase.
        /// ptas_incomeretirement_base.
        /// </summary>
        double? IncomeRetirementBase { get; set; }

        /// <summary>
        /// Gets or sets a value for SchdePropType.
        /// ptas_schdeproptype.
        /// </summary>
        string SchdePropType { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeUnemployment.
        /// ptas_incomeunemployment.
        /// </summary>
        double? IncomeUnemployment { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017alimonyrcvd.
        /// ptas_10402017alimonyrcvd.
        /// </summary>
        double? F10402017alimonyrcvd { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018ssBenefits.
        /// ptas_10402018ssbenefits.
        /// </summary>
        double? F10402018ssBenefits { get; set; }

        /// <summary>
        /// Gets or sets a value for SchdeDepreciation.
        /// ptas_schdedepreciation.
        /// </summary>
        double? SchdeDepreciation { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017RoyalTiesTrustsetcBase.
        /// ptas_10402017royaltiestrustsetc_base.
        /// </summary>
        double? F10402017RoyalTiesTrustsetcBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeGambling.
        /// ptas_incomegambling.
        /// </summary>
        double? IncomeGambling { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018Wages.
        /// ptas_10402018wages.
        /// </summary>
        double? F10402018Wages { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeDividendsBase.
        /// ptas_incomedividends_base.
        /// </summary>
        double? IncomeDividendsBase { get; set; }

        /// <summary>
        /// Gets or sets a value for TotalExpensesBase.
        /// ptas_totalexpenses_base.
        /// </summary>
        double? TotalExpensesBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeSocialSec.
        /// ptas_incomesocialsec.
        /// </summary>
        double? IncomeSocialSec { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040ezTaxInterestBase.
        /// ptas_1040eztaxinterest_base.
        /// </summary>
        double? F1040ezTaxInterestBase { get; set; }

        /// <summary>
        /// Gets or sets a value for TotalExpenses.
        /// ptas_totalexpenses.
        /// </summary>
        double? TotalExpenses { get; set; }

        /// <summary>
        /// Gets or sets a value for SchdCotherIncomeAmt.
        /// ptas_schdcotherincomeamt.
        /// </summary>
        double? SchdCotherIncomeAmt { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018IraPensionsTax.
        /// ptas_10402018irapensionstax.
        /// </summary>
        double? F10402018IraPensionsTax { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aSocialSecBase.
        /// ptas_1040asocialsec_base.
        /// </summary>
        double? F1040aSocialSecBase { get; set; }

        /// <summary>
        /// Gets or sets a value for SchdcNetProfitLoss.
        /// ptas_schdcnetprofitloss.
        /// </summary>
        double? SchdcNetProfitLoss { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddShortTermBoxaBase.
        /// ptas_schddshorttermboxa_base.
        /// </summary>
        double? SchddShortTermBoxaBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017TaxExInterest.
        /// ptas_10402017taxexinterest.
        /// </summary>
        double? F10402017TaxExInterest { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018ssBenefitsBase.
        /// ptas_10402018ssbenefits_base.
        /// </summary>
        double? F10402018ssBenefitsBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeWagesBase.
        /// ptas_incomewages_base.
        /// </summary>
        double? IncomeWagesBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017WagesBase.
        /// ptas_10402017wages_base.
        /// </summary>
        double? F10402017WagesBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017WagesBase.
        /// ptas_10402017bizincomeorloss.
        /// </summary>
        double? F10402017BizIncomeOrLoss { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017PensionsAnnuties.
        /// ptas_10402017pensionsannuties.
        /// </summary>
        double? F10402017PensionsAnnuties { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040AiraDistTax.
        /// ptas_1040airadisttax.
        /// </summary>
        double? F1040AiraDistTax { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aUnemploymentBase.
        /// ptas_1040aunemployment_base.
        /// </summary>
        double? F1040aUnemploymentBase { get; set; }

        /// <summary>
        /// Gets or sets a value for NetTotalIncome.
        /// ptas_nettotalincome.
        /// </summary>
        double? NetTotalIncome { get; set; }

        /// <summary>
        /// Gets or sets a value for Name.
        /// ptas_name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddLongTermboxbBase.
        /// ptas_schddlongtermboxb_base.
        /// </summary>
        double? SchddLongTermboxbBase { get; set; }

        /// <summary>
        /// Gets or sets a value for FilerType.
        /// ptas_filertype.
        /// </summary>
        int? FilerType { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aTaxexInterestBase.
        /// ptas_1040ataxexinterest_base.
        /// </summary>
        double? F1040aTaxexInterestBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017ssBenefitsBase.
        /// ptas_10402017ssbenefits_base.
        /// </summary>
        double? F10402017ssBenefitsBase { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1CapGainLossBase.
        /// ptas_schd1capgainloss_base.
        /// </summary>
        double? Schd1CapGainLossBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeRentalBase.
        /// ptas_incomerental_base.
        /// </summary>
        double? IncomeRentalBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017bizIncomeOrLossBase.
        /// ptas_10402017bizincomeorloss_base.
        /// </summary>
        double? F10402017bizIncomeOrLossBase { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1RentalRealEstate.
        /// ptas_schd1rentalrealestate.
        /// </summary>
        double? Schd1RentalRealEstate { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aCapGains.
        /// ptas_1040acapgains.
        /// </summary>
        double? F1040aCapGains { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1alimonyBase.
        /// ptas_schd1alimony_base.
        /// </summary>
        double? Schd1alimonyBase { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1RentalRealEstateBase.
        /// ptas_schd1rentalrealestate_base.
        /// </summary>
        double? Schd1RentalRealEstateBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017TaxRefunds.
        /// ptas_10402017taxrefunds.
        /// </summary>
        double? F10402017TaxRefunds { get; set; }

        /// <summary>
        /// Gets or sets a value for ExpenseNursingHome.
        /// ptas_expensenursinghome.
        /// </summary>
        double? ExpenseNursingHome { get; set; }

        /// <summary>
        /// Gets or sets a value for SchdcDepreciation.
        /// ptas_schdcdepreciation.
        /// </summary>
        double? SchdcDepreciation { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeDisabilitySrc.
        /// ptas_incomedisabilitysrc.
        /// </summary>
        int? IncomeDisabilitySrc { get; set; }

        /// <summary>
        /// Gets or sets a value for ExpensesInHome.
        /// ptas_expensesinhome.
        /// </summary>
        double? ExpensesInHome { get; set; }

        /// <summary>
        /// Gets or sets a value for MedicarePlanId.
        /// _ptas_medicareplanid_value.
        /// </summary>
        string MedicarePlanId { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1alimony.
        /// ptas_schd1alimony.
        /// </summary>
        double? Schd1alimony { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1farmIncomeLoss.
        /// ptas_schd1farmincomeloss.
        /// </summary>
        double? Schd1farmIncomeLoss { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018TaxInterestBase.
        /// ptas_10402018taxinterest_base.
        /// </summary>
        double? F10402018TaxInterestBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017TaxRefundsBase.
        /// ptas_10402017taxrefunds_base.
        /// </summary>
        double? F10402017TaxRefundsBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aTaxInterestBase.
        /// ptas_1040ataxinterest_base.
        /// </summary>
        double? F1040aTaxInterestBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeRental.
        /// ptas_incomerental.
        /// </summary>
        double? IncomeRental { get; set; }

        /// <summary>
        /// Gets or sets a value for ExpensePres.
        /// ptas_expensepres.
        /// </summary>
        double? ExpensePres { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1BusinessIncomeLoss.
        /// ptas_schd1businessincomeloss.
        /// </summary>
        double? Schd1BusinessIncomeLoss { get; set; }

        /// <summary>
        /// Gets or sets a value for SchdcDepreciationBase.
        /// ptas_schdcdepreciation_base.
        /// </summary>
        double? SchdcDepreciationBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017UnemploymentBase.
        /// ptas_10402017unemployment_base.
        /// </summary>
        double? F10402017UnemploymentBase { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddShortTermBoxbBase.
        /// ptas_schddshorttermboxb_base.
        /// </summary>
        double? SchddShortTermBoxbBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F8949TotalGains.
        /// ptas_8949totalgains.
        /// </summary>
        double? F8949TotalGains { get; set; }

        /// <summary>
        /// Gets or sets a value for SchdcSqftBusiness.
        /// ptas_schdcsqftbusiness.
        /// </summary>
        int? SchdcSqftBusiness { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddLongTermBoxa.
        /// ptas_schddlongtermboxa.
        /// </summary>
        double? SchddLongTermBoxa { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aPensionsTaxBase.
        /// ptas_1040apensionstax_base.
        /// </summary>
        double? F1040aPensionsTaxBase { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddShortTermBoxc.
        /// ptas_schddshorttermboxc.
        /// </summary>
        double? SchddShortTermBoxc { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1OtherIncomeAmtBase.
        /// ptas_schd1otherincomeamt_base.
        /// </summary>
        double? Schd1OtherIncomeAmtBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040ezUnemploymentBase.
        /// ptas_1040ezunemployment_base.
        /// </summary>
        double? F1040ezUnemploymentBase { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddRequired.
        /// ptas_schddrequired.
        /// </summary>
        bool? SchddRequired { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddLongTermBoxaBase.
        /// ptas_schddlongtermboxa_base.
        /// </summary>
        double? SchddLongTermBoxaBase { get; set; }

        /// <summary>
        /// Gets or sets a value for NetTotalIncomeBase.
        /// ptas_nettotalincome_base.
        /// </summary>
        double? NetTotalIncomeBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017PensionsAnnutiesBase.
        /// ptas_10402017pensionsannuties_base.
        /// </summary>
        double? F10402017PensionsAnnutiesBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeSocialSecBase.
        /// ptas_incomesocialsec_base.
        /// </summary>
        double? IncomeSocialSecBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aOrdDividends.
        /// ptas_1040aorddividends.
        /// </summary>
        double? F1040aOrdDividends { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018TotalIncome.
        /// ptas_10402018totalincome.
        /// </summary>
        double? F10402018TotalIncome { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddShortTermBoxb.
        /// ptas_schddshorttermboxb.
        /// </summary>
        double? SchddShortTermBoxb { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1TaxRefundsBase.
        /// ptas_schd1taxrefunds_base.
        /// </summary>
        double? Schd1TaxRefundsBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018IraPensions.
        /// ptas_10402018irapensions.
        /// </summary>
        double? F10402018IraPensions { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1OtherGainsLoss.
        /// ptas_schd1othergainsloss.
        /// </summary>
        double? Schd1OtherGainsLoss { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017Wages.
        /// ptas_10402017wages.
        /// </summary>
        double? F10402017Wages { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aWagesBase.
        /// ptas_1040awages_base.
        /// </summary>
        double? F1040aWagesBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeCapGains_base.
        /// ptas_incomecapgains_base.
        /// </summary>
        double? IncomeCapGains_base { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1BusinessIncomeLossBase.
        /// ptas_schd1businessincomeloss_base.
        /// </summary>
        double? Schd1BusinessIncomeLossBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017CapGainLossBase.
        /// ptas_10402017capgainloss_base.
        /// </summary>
        double? F10402017CapGainLossBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017DeductionsBase.
        /// ptas_10402017deductions_base.
        /// </summary>
        double? F10402017DeductionsBase { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddnetShortTermGains.
        /// ptas_schddnetshorttermgains.
        /// </summary>
        double? SchddnetShortTermGains { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddShortTerm6252.
        /// ptas_schddshortterm6252.
        /// </summary>
        double? SchddShortTerm6252 { get; set; }

        /// <summary>
        /// Gets or sets a value for Modified By.
        /// _modifiedby_value.
        /// </summary>
        Guid? ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017IraDistTaxBase.
        /// ptas_10402017iradisttax_base.
        /// </summary>
        double? F10402017IraDistTaxBase { get; set; }

        /// <summary>
        /// Gets or sets a value for ExpenseNursingHomeBase.
        /// ptas_expensenursinghome_base.
        /// </summary>
        double? ExpenseNursingHomeBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aTaxInterest.
        /// ptas_1040ataxinterest.
        /// </summary>
        double? F1040aTaxInterest { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aTaxRefundsBase.
        /// ptas_1040ataxrefunds_base.
        /// </summary>
        double? F1040aTaxRefundsBase { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddCapGainsBase.
        /// ptas_schddcapgains_base.
        /// </summary>
        double? SchddCapGainsBase { get; set; }

        /// <summary>
        /// Gets or sets a value for SchdcSqftHome.
        /// ptas_schdcsqfthome.
        /// </summary>
        int? SchdcSqftHome { get; set; }

        /// <summary>
        /// Gets or sets a value for ExpenseInHomePayee.
        /// ptas_expenseinhomepayee.
        /// </summary>
        string ExpenseInHomePayee { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017PenannuitiesTax.
        /// ptas_10402017penannuitiestax.
        /// </summary>
        double? F10402017PenannuitiesTax { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeServiceDisability.
        /// ptas_incomeservicedisability.
        /// </summary>
        double? IncomeServiceDisability { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeBusiness.
        /// ptas_incomebusiness.
        /// </summary>
        double? IncomeBusiness { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aPensions.
        /// ptas_1040apensions.
        /// </summary>
        double? F1040aPensions { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017TaxexInterestBase.
        /// ptas_10402017taxexinterest_base.
        /// </summary>
        double? F10402017TaxexInterestBase { get; set; }

        /// <summary>
        /// Gets or sets a value for SchdcexBusinessHome.
        /// ptas_schdcexbusinesshome.
        /// </summary>
        double? SchdcexBusinessHome { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017PenannuitiesTaxBase.
        /// ptas_10402017penannuitiestax_base.
        /// </summary>
        double? F10402017PenannuitiesTaxBase { get; set; }

        /// <summary>
        /// Gets or sets a value for TotalIncome.
        /// ptas_totalincome.
        /// </summary>
        double? TotalIncome { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aUnemployment.
        /// ptas_1040aunemployment.
        /// </summary>
        double? F1040aUnemployment { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017alimonyrcvdBase.
        /// ptas_10402017alimonyrcvd_base.
        /// </summary>
        double? F10402017alimonyrcvdBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeDisabilityBase.
        /// ptas_incomedisability_base.
        /// </summary>
        double? IncomeDisabilityBase { get; set; }

        /// <summary>
        /// Gets or sets a value for TotalIncomeBase.
        /// ptas_totalincome_base.
        /// </summary>
        double? TotalIncomeBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aIraDistTaxBase.
        /// ptas_1040airadisttax_base.
        /// </summary>
        double? F1040aIraDistTaxBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aCapGainsBase.
        /// ptas_1040acapgains_base.
        /// </summary>
        double? F1040aCapGainsBase { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1TaxRefunds.
        /// ptas_schd1taxrefunds.
        /// </summary>
        double? Schd1TaxRefunds { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017OtherIncomeType.
        /// ptas_10402017otherincometype.
        /// </summary>
        string F10402017OtherIncomeType { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017OtherGainsLossBase.
        /// ptas_10402017othergainsloss_base.
        /// </summary>
        double? F10402017OtherGainsLossBase { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddLongTermboxc.
        /// ptas_schddlongtermboxc.
        /// </summary>
        double? SchddLongTermboxc { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018AdjgrossIncomeBase.
        /// ptas_10402018adjgrossincome_base.
        /// </summary>
        double? F10402018AdjgrossIncomeBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017Deductions.
        /// ptas_10402017deductions.
        /// </summary>
        double? F10402017Deductions { get; set; }

        /// <summary>
        /// Gets or sets a value for ExpensePresBase.
        /// ptas_expensepres_base.
        /// </summary>
        double? ExpensePresBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeGifts.
        /// ptas_incomegifts.
        /// </summary>
        double? IncomeGifts { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeIraTaxAmtBase.
        /// ptas_incomeirataxamt_base.
        /// </summary>
        double? IncomeIraTaxAmtBase { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddShortTermBoxa.
        /// ptas_schddshorttermboxa.
        /// </summary>
        double? SchddShortTermBoxa { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeCoownerBase.
        /// ptas_incomecoowner_base.
        /// </summary>
        double? IncomeCoownerBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeCoowner.
        /// ptas_incomecoowner.
        /// </summary>
        double? IncomeCoowner { get; set; }

        /// <summary>
        /// Gets or sets a value for ExpenseOther.
        /// ptas_expenseother.
        /// </summary>
        double? ExpenseOther { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeAlimonyBase.
        /// ptas_incomealimony_base.
        /// </summary>
        double? IncomeAlimonyBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017Unemployment.
        /// ptas_10402017unemployment.
        /// </summary>
        double? F10402017Unemployment { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017FarmIncomeLossBase.
        /// ptas_10402017farmincomeloss_base.
        /// </summary>
        double? F10402017FarmIncomeLossBase { get; set; }

        /// <summary>
        /// Gets or sets a value for SchdeDepreciationBase.
        /// ptas_schdedepreciation_base.
        /// </summary>
        double? SchdeDepreciationBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018IraPensionsBase.
        /// ptas_10402018irapensions_base.
        /// </summary>
        double? F10402018IraPensionsBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017ssBenefits.
        /// ptas_10402017ssbenefits.
        /// </summary>
        double? F10402017ssBenefits { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aPensionsTax.
        /// ptas_1040apensionstax.
        /// </summary>
        double? F1040aPensionsTax { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeCapGains.
        /// ptas_incomecapgains.
        /// </summary>
        double? IncomeCapGains { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017DividendsBase.
        /// ptas_10402017dividends_base.
        /// </summary>
        double? F10402017DividendsBase { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1OtherIncomeType.
        /// ptas_schd1otherincometype.
        /// </summary>
        string Schd1OtherIncomeType { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aSocialSec.
        /// ptas_1040asocialsec.
        /// </summary>
        double? F1040aSocialSec { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017IraDistTax.
        /// ptas_10402017iradisttax.
        /// </summary>
        double? F10402017IraDistTax { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeAlimony.
        /// ptas_incomealimony.
        /// </summary>
        double? IncomeAlimony { get; set; }

        /// <summary>
        /// Gets or sets a value for SchdcexBusinessHomeBase.
        /// ptas_schdcexbusinesshome_base.
        /// </summary>
        double? SchdcexBusinessHomeBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeDshs.
        /// ptas_incomedshs.
        /// </summary>
        double? IncomeDshs { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeCountries.
        /// ptas_incomecountries.
        /// </summary>
        double? IncomeCountries { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017OtherGainsLoss.
        /// ptas_10402017othergainsloss.
        /// </summary>
        double? F10402017OtherGainsLoss { get; set; }

        /// <summary>
        /// Gets or sets a value for ExpensesInHomeBase.
        /// ptas_expensesinhome_base.
        /// </summary>
        double? ExpensesInHomeBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018TaxexInterestBase.
        /// ptas_10402018taxexinterest_base.
        /// </summary>
        double? F10402018TaxexInterestBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeOtherBase.
        /// ptas_incomeother_base.
        /// </summary>
        double? IncomeOtherBase { get; set; }

        /// <summary>
        /// Gets or sets a value for SchdfDepreciationBase.
        /// ptas_schdfdepreciation_base.
        /// </summary>
        double? SchdfDepreciationBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018IraPensionsTaxBase.
        /// ptas_10402018irapensionstax_base.
        /// </summary>
        double? F10402018IraPensionsTaxBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeOther.
        /// ptas_incomeother.
        /// </summary>
        double? IncomeOther { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeRetirement.
        /// ptas_incomeretirement.
        /// </summary>
        double? IncomeRetirement { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1OtherIncomeAmt.
        /// ptas_schd1otherincomeamt.
        /// </summary>
        double? Schd1OtherIncomeAmt { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aWages.
        /// ptas_1040awages.
        /// </summary>
        double? F1040aWages { get; set; }

        /// <summary>
        /// Gets or sets a value for SchdephysAddress.
        /// ptas_schdephysaddress.
        /// </summary>
        string SchdephysAddress { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aDeductions.
        /// ptas_1040adeductions.
        /// </summary>
        double? F1040aDeductions { get; set; }

        /// <summary>
        /// Gets or sets a value for ExpenseMedicarePlanBase.
        /// ptas_expensemedicareplan_base.
        /// </summary>
        double? ExpenseMedicarePlanBase { get; set; }

        /// <summary>
        /// Gets or sets a value for ExpenseAssistedLivingBase.
        /// ptas_expenseassistedliving_base.
        /// </summary>
        double? ExpenseAssistedLivingBase { get; set; }

        /// <summary>
        /// Gets or sets a value for Schd1OtherGainsLossBase.
        /// ptas_schd1othergainsloss_base.
        /// </summary>
        double? Schd1OtherGainsLossBase { get; set; }

        /// <summary>
        /// Gets or sets a value for IncomeExpenses.
        /// ptas_incomeexpenses.
        /// </summary>
        double? IncomeExpenses { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017TaxInterestBase.
        /// ptas_10402017taxinterest_base.
        /// </summary>
        double? F10402017TaxInterestBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aTaxexInterest.
        /// ptas_1040ataxexinterest.
        /// </summary>
        double? F1040aTaxexInterest { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040aTaxexInterest.
        /// ptas_expenseassistedlivingpayee.
        /// </summary>
        string ExpenseAssistedLivingPayee { get; set; }

        /// <summary>
        /// Gets or sets a value for ExpenseAssistedLiving.
        /// ptas_expenseassistedliving.
        /// </summary>
        double? ExpenseAssistedLiving { get; set; }

        /// <summary>
        /// Gets or sets a value for F1040ezUnemployment.
        /// ptas_1040ezunemployment.
        /// </summary>
        double? F1040ezUnemployment { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017TaxInterest.
        /// ptas_10402017taxinterest.
        /// </summary>
        double? F10402017TaxInterest { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017FarmIncomeLoss.
        /// ptas_10402017farmincomeloss.
        /// </summary>
        double? F10402017FarmIncomeLoss { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402017OtherIncomeBase.
        /// ptas_10402017otherincome_base.
        /// </summary>
        double? F10402017OtherIncomeBase { get; set; }

        /// <summary>
        /// Gets or sets a value for F10402018WagesBase.
        /// ptas_10402018wages_base.
        /// </summary>
        double? F10402018WagesBase { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddShortTermBoxc_base.
        /// ptas_schddshorttermboxc_base.
        /// </summary>
        double? SchddShortTermBoxc_base { get; set; }

        /// <summary>
        /// Gets or sets a value for SchddnetShortTermGainsBase.
        /// ptas_schddnetshorttermgains_base.
        /// </summary>
        double? SchddnetShortTermGainsBase { get; set; }

        /// <summary>
        /// Gets or sets fake field for a N:N relationship that is implemented as 1:N.
        /// </summary>
        string OccupantId { get; set; }

        /// <summary>
        /// Gets or sets a value for TrustPartnershipEstateIncome.
        /// </summary>
        double? TrustPartnershipEstateIncome { get; set; }

        /// <summary>
        /// Gets or sets a value for TaxNonTaxbonds.
        /// </summary>
        double? TaxNonTaxbonds { get; set; }

        /// <summary>
        /// Gets or sets incomeDshs_base.
        /// </summary>
        double? IncomeDshs_base { get; set; }
    }
}
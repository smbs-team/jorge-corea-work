// <copyright file="DynamicsSeniorExemptionApplicationFinancial.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.JSONMappings
{
    using System;
    using Newtonsoft.Json;
    using PTASDynamicsCrudHelperClasses.Interfaces;

    /// <summary>
    /// Senior Exemption Application Financial info returned by dynamics.
    /// </summary>
    public class DynamicsSeniorExemptionApplicationFinancial : ISeniorExemptionApplicationFinancial
    {
        /// <inheritdoc/>
        [JsonProperty("ptas_sefinancialformsid")]
        public string SEFinancialFormsId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_alternatekey")]
        public int? AlternateKey { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_ptas_seappdetailid_value", NullValueHandling = NullValueHandling.Ignore)]
        public string SEAppDetailId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_ptas_yearid_value", NullValueHandling = NullValueHandling.Ignore)]
        public string YearId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_financialformtype")]
        public int? FinancialFormType { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040ezwages_base")]
        public double? F1040EXWagesBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1farmincomeloss_base")]
        public double? Schd1FarmIncomeLossBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomewages")]
        public double? IncomeWages { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddcapgains")]
        public double? SchddCapgains { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017royaltiestrustsetc")]
        public double? F10402017RoyalTiesTrustSetc { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040ataxrefunds")]
        public double? F1040aTaxRefunds { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018taxinterest")]
        public double? F10402018TaxInterest { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017capgainloss")]
        public double? F10402017CapGainLoss { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomeservicedisability_base")]
        public double? IncomeServiceDisabilityBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040apensions_base")]
        public double? F1040aPensionsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1unemployment_base")]
        public double? Schd1UnemploymentBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_modifiedonbehalfby_value", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? ModifiedOnBehalfBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_8949totalgains_base")]
        public double? F8949totalgains_base { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schdcnetprofitloss_base")]
        public double? SchdCnetProfitLossBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomeexpenses_base")]
        public double? IncomeExpensesBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040eztaxinterest")]
        public double? F1040eztaxinterest { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_createdonbehalfby_value", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? CreatedOnBehalfBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_createdby_value", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? CreatedBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018dividends")]
        public double? F10402018Dividends { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomegambling_base")]
        public double? IncomeGamblingBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomeunemployment_base")]
        public double? IncomeUnemploymentBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomegifts_base")]
        public double? IncomeGiftsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018dividends_base")]
        public double? F10402018DividendsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040aorddividends_base")]
        public double? F1040aordDividendsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schdfdepreciation")]
        public double? SchdfDepreciation { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_8829percentusedforbusiness")]
        public int? F8829PercentUsedForBusiness { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018taxexinterest")]
        public double? F10402018TaxEXInterest { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddlongtermboxb")]
        public double? SchddLongTermBoxb { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040ezwages")]
        public double? F1040EZWages { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomedividends")]
        public double? IncomeDividends { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_expenseother_base")]
        public double? ExpenseOtherBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018totalincome_base")]
        public double? F10402018TotalIncomeBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017otherincome")]
        public double? F10402017OtherIncome { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomecountries_base")]
        public double? IncomeCountriesBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomeirataxamt")]
        public double? IncomeIraTaxAmt { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018adjgrossincome")]
        public double? F10402018adjgrossincome { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1unemployment")]
        public double? Schd1Unemployment { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1capgainloss")]
        public double? Schd1CapGainLoss { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_expensemedicareplan")]
        public double? ExpenseMedicarePlan { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddlongtermboxc_base")]
        public double? SchddLongTermBoxcBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040adeductions_base")]
        public double? F1040aDeductionsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017dividends")]
        public double? F10402017Dividends { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_expensenursingpayee")]
        public string ExpenseNursingPayee { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddshortterm6252_base")]
        public double? SchddShortTerm6252Base { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomebusiness_base")]
        public double? IncomeBusinessBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schdcotherincomeamt_base")]
        public double? SchdcOtherIncomeAmtBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomedisability")]
        public double? IncomeDisability { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomeretirement_base")]
        public double? IncomeRetirementBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schdeproptype")]
        public string SchdePropType { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomeunemployment")]
        public double? IncomeUnemployment { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017alimonyrcvd")]
        public double? F10402017alimonyrcvd { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018ssbenefits")]
        public double? F10402018ssBenefits { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schdedepreciation")]
        public double? SchdeDepreciation { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017royaltiestrustsetc_base")]
        public double? F10402017RoyalTiesTrustsetcBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomegambling")]
        public double? IncomeGambling { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018wages")]
        public double? F10402018Wages { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomedividends_base")]
        public double? IncomeDividendsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_totalexpenses_base")]
        public double? TotalExpensesBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomesocialsec")]
        public double? IncomeSocialSec { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040eztaxinterest_base")]
        public double? F1040ezTaxInterestBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_totalexpenses")]
        public double? TotalExpenses { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schdcotherincomeamt")]
        public double? SchdCotherIncomeAmt { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018irapensionstax")]
        public double? F10402018IraPensionsTax { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040asocialsec_base")]
        public double? F1040aSocialSecBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schdcnetprofitloss")]
        public double? SchdcNetProfitLoss { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddshorttermboxa_base")]
        public double? SchddShortTermBoxaBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017taxexinterest")]
        public double? F10402017TaxExInterest { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018ssbenefits_base")]
        public double? F10402018ssBenefitsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomewages_base")]
        public double? IncomeWagesBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017wages_base")]
        public double? F10402017WagesBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017bizincomeorloss")]
        public double? F10402017BizIncomeOrLoss { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017pensionsannuties")]
        public double? F10402017PensionsAnnuties { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040airadisttax")]
        public double? F1040AiraDistTax { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040aunemployment_base")]
        public double? F1040aUnemploymentBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_nettotalincome")]
        public double? NetTotalIncome { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_name")]
        public string Name { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddlongtermboxb_base")]
        public double? SchddLongTermboxbBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_filertype")]
        public int? FilerType { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040ataxexinterest_base")]
        public double? F1040aTaxexInterestBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017ssbenefits_base")]
        public double? F10402017ssBenefitsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1capgainloss_base")]
        public double? Schd1CapGainLossBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomerental_base")]
        public double? IncomeRentalBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017bizincomeorloss_base")]
        public double? F10402017bizIncomeOrLossBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1rentalrealestate")]
        public double? Schd1RentalRealEstate { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040acapgains")]
        public double? F1040aCapGains { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1alimony_base")]
        public double? Schd1alimonyBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1rentalrealestate_base")]
        public double? Schd1RentalRealEstateBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017taxrefunds")]
        public double? F10402017TaxRefunds { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_expensenursinghome")]
        public double? ExpenseNursingHome { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schdcdepreciation")]
        public double? SchdcDepreciation { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomedisabilitysrc")]
        public int? IncomeDisabilitySrc { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_expensesinhome")]
        public double? ExpensesInHome { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_ptas_medicareplanid_value", NullValueHandling = NullValueHandling.Ignore)]
        public string MedicarePlanId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1alimony")]
        public double? Schd1alimony { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1farmincomeloss")]
        public double? Schd1farmIncomeLoss { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018taxinterest_base")]
        public double? F10402018TaxInterestBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017taxrefunds_base")]
        public double? F10402017TaxRefundsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040ataxinterest_base")]
        public double? F1040aTaxInterestBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomerental")]
        public double? IncomeRental { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_expensepres")]
        public double? ExpensePres { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1businessincomeloss")]
        public double? Schd1BusinessIncomeLoss { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schdcdepreciation_base")]
        public double? SchdcDepreciationBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017unemployment_base")]
        public double? F10402017UnemploymentBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddshorttermboxb_base")]
        public double? SchddShortTermBoxbBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_8949totalgains")]
        public double? F8949TotalGains { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schdcsqftbusiness")]
        public int? SchdcSqftBusiness { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddlongtermboxa")]
        public double? SchddLongTermBoxa { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040apensionstax_base")]
        public double? F1040aPensionsTaxBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddshorttermboxc")]
        public double? SchddShortTermBoxc { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1otherincomeamt_base")]
        public double? Schd1OtherIncomeAmtBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040ezunemployment_base")]
        public double? F1040ezUnemploymentBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddrequired")]
        public bool? SchddRequired { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddlongtermboxa_base")]
        public double? SchddLongTermBoxaBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_nettotalincome_base")]
        public double? NetTotalIncomeBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017pensionsannuties_base")]
        public double? F10402017PensionsAnnutiesBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomesocialsec_base")]
        public double? IncomeSocialSecBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040aorddividends")]
        public double? F1040aOrdDividends { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018totalincome")]
        public double? F10402018TotalIncome { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddshorttermboxb")]
        public double? SchddShortTermBoxb { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1taxrefunds_base")]
        public double? Schd1TaxRefundsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018irapensions")]
        public double? F10402018IraPensions { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1othergainsloss")]
        public double? Schd1OtherGainsLoss { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017wages")]
        public double? F10402017Wages { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040awages_base")]
        public double? F1040aWagesBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomedshs_base")]
        public double? IncomeDshs_base { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomecapgains_base")]
        public double? IncomeCapGains_base { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1businessincomeloss_base")]
        public double? Schd1BusinessIncomeLossBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017capgainloss_base")]
        public double? F10402017CapGainLossBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017deductions_base")]
        public double? F10402017DeductionsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddnetshorttermgains")]
        public double? SchddnetShortTermGains { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddshortterm6252")]
        public double? SchddShortTerm6252 { get; set; }

        /// <inheritdoc/>
        [JsonProperty("_modifiedby_value", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? ModifiedBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017iradisttax_base")]
        public double? F10402017IraDistTaxBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_expensenursinghome_base")]
        public double? ExpenseNursingHomeBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040ataxinterest")]
        public double? F1040aTaxInterest { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040ataxrefunds_base")]
        public double? F1040aTaxRefundsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddcapgains_base")]
        public double? SchddCapGainsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schdcsqfthome")]
        public int? SchdcSqftHome { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_expenseinhomepayee")]
        public string ExpenseInHomePayee { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017penannuitiestax")]
        public double? F10402017PenannuitiesTax { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomeservicedisability")]
        public double? IncomeServiceDisability { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomebusiness")]
        public double? IncomeBusiness { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040apensions")]
        public double? F1040aPensions { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017taxexinterest_base")]
        public double? F10402017TaxexInterestBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schdcexbusinesshome")]
        public double? SchdcexBusinessHome { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017penannuitiestax_base")]
        public double? F10402017PenannuitiesTaxBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_totalincome")]
        public double? TotalIncome { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040aunemployment")]
        public double? F1040aUnemployment { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017alimonyrcvd_base")]
        public double? F10402017alimonyrcvdBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomedisability_base")]
        public double? IncomeDisabilityBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_totalincome_base")]
        public double? TotalIncomeBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040airadisttax_base")]
        public double? F1040aIraDistTaxBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040acapgains_base")]
        public double? F1040aCapGainsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1taxrefunds")]
        public double? Schd1TaxRefunds { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017otherincometype")]
        public string F10402017OtherIncomeType { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017othergainsloss_base")]
        public double? F10402017OtherGainsLossBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddlongtermboxc")]
        public double? SchddLongTermboxc { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018adjgrossincome_base")]
        public double? F10402018AdjgrossIncomeBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017deductions")]
        public double? F10402017Deductions { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_expensepres_base")]
        public double? ExpensePresBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomegifts")]
        public double? IncomeGifts { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomeirataxamt_base")]
        public double? IncomeIraTaxAmtBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddshorttermboxa")]
        public double? SchddShortTermBoxa { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomecoowner_base")]
        public double? IncomeCoownerBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomecoowner")]
        public double? IncomeCoowner { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_expenseother")]
        public double? ExpenseOther { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomealimony_base")]
        public double? IncomeAlimonyBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017unemployment")]
        public double? F10402017Unemployment { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017farmincomeloss_base")]
        public double? F10402017FarmIncomeLossBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schdedepreciation_base")]
        public double? SchdeDepreciationBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018irapensions_base")]
        public double? F10402018IraPensionsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017ssbenefits")]
        public double? F10402017ssBenefits { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040apensionstax")]
        public double? F1040aPensionsTax { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomecapgains")]
        public double? IncomeCapGains { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017dividends_base")]
        public double? F10402017DividendsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1otherincometype")]
        public string Schd1OtherIncomeType { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040asocialsec")]
        public double? F1040aSocialSec { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017iradisttax")]
        public double? F10402017IraDistTax { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomealimony")]
        public double? IncomeAlimony { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schdcexbusinesshome_base")]
        public double? SchdcexBusinessHomeBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomedshs")]
        public double? IncomeDshs { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomecountries")]
        public double? IncomeCountries { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017othergainsloss")]
        public double? F10402017OtherGainsLoss { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_expensesinhome_base")]
        public double? ExpensesInHomeBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018taxexinterest_base")]
        public double? F10402018TaxexInterestBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomeother_base")]
        public double? IncomeOtherBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schdfdepreciation_base")]
        public double? SchdfDepreciationBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018irapensionstax_base")]
        public double? F10402018IraPensionsTaxBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomeother")]
        public double? IncomeOther { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomeretirement")]
        public double? IncomeRetirement { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1otherincomeamt")]
        public double? Schd1OtherIncomeAmt { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040awages")]
        public double? F1040aWages { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schdephysaddress")]
        public string SchdephysAddress { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040adeductions")]
        public double? F1040aDeductions { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_expensemedicareplan_base")]
        public double? ExpenseMedicarePlanBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_expenseassistedliving_base")]
        public double? ExpenseAssistedLivingBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schd1othergainsloss_base")]
        public double? Schd1OtherGainsLossBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_incomeexpenses")]
        public double? IncomeExpenses { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017taxinterest_base")]
        public double? F10402017TaxInterestBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040ataxexinterest")]
        public double? F1040aTaxexInterest { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_expenseassistedlivingpayee")]
        public string ExpenseAssistedLivingPayee { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_expenseassistedliving")]
        public double? ExpenseAssistedLiving { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_1040ezunemployment")]
        public double? F1040ezUnemployment { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017taxinterest")]
        public double? F10402017TaxInterest { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017farmincomeloss")]
        public double? F10402017FarmIncomeLoss { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402017otherincome_base")]
        public double? F10402017OtherIncomeBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_10402018wages_base")]
        public double? F10402018WagesBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddshorttermboxc_base")]
        public double? SchddShortTermBoxc_base { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_schddnetshorttermgains_base")]
        public double? SchddnetShortTermGainsBase { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_occupantid")]
        public string OccupantId { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_trustpartnershipestateincome")]
        public double? TrustPartnershipEstateIncome { get; set; }

        /// <inheritdoc/>
        [JsonProperty("ptas_taxnontaxbonds")]
        public double? TaxNonTaxbonds { get; set; }
    }
}

var financesCommands = {
  addExpenses: function(userData, file) {
    var { ENTER, SPACE } = this.api.Keys;
    return this.setValue('@assistedLivingInput', userData.ASSISTED_LIVING)
      .setValue('@inHomeCareInput', userData.IN_HOME_CARE)
      .setValue(
        '@nonReimbursedPrescriptionsInput',
        userData.NON_REIMBURSED_PRESCRIPTIONS
      )
      .setValue('@approvedMedicareInput', userData.APPROVED_MEDICARE)
      .pause(1000)
      .click('@medicareProviderSelectInput')
      .pause(1000)
      .advClick(userData.EXPENSES_DATA.PROVIDER, ENTER)
      .pause(1000)
      .click('@mediaPlanSelectInput')
      .pause(1000)
      .advClick(userData.EXPENSES_DATA.PLAN, ENTER)
      .pause(1000)
      .uploadImage('@yearExpenseFileInput', file)
      .sendKeys('@continueButton', ENTER);
  },
  addFinancialInfo: function(data, file) {
    var { ENTER, SPACE } = this.api.Keys;
    return this.waitForElementVisible('@netSocial', 30000)
      .setValue('@netSocial', data.NET_SOCIAL_SECURITY_LESS_MEDIACARE)
      .setValue('@interestDividends', data.INTEREST_DIVIDENDS)
      .setValue('@retirementPension', data.PENSIONS_AND_ANNUITIES)
      .setValue('@iraTaxableAmount', data.IRAS)
      .setValue('@wages', data.WAGES)
      .setValue('@capitalGains', data.CAPITAL_GAINS)
      .setValue('@businessIncome', data.BUSINESS_INCOME)
      .setValue('@otherIncome', data.OTHER_GAINS)
      .setValue('@rentalIncome', data.RENTAL)
      .setValue('@gifts', data.GIFTS)
      .setValue('@otherCountriesIncome', data.INCOME_FROM_OTHER_COUNTRIES)
      .setValue('@unemployment', data.UNEMPLOYMENT)
      .setValue('@publicAssistance', data.PUBLIC_ASSISTANCE)
      .setValue('@gamblingIncome', data.GAMBLING)
      .setValue('@disabilityIncome', data.DISABILITY_INCOME)
      .setValue('@serviceRelatedDisabilityIncome', data.VA_BENEFIT)
      .pause(2000)
      .click('@disabilitySourceSelect')
      .pause(1000)
      .advClick(data.DISABILITY_SOURCE.SOCIAL_SECURITY)
      .pause(1000)
      .setValue('@trustRoyaltyIncome', data.ROYAL_INCOME)
      .setValue('@bonds', data.BONDS_INCOME)
      .setValue('@alimony', data.ALIMONY)
      .uploadImage('@uploadFile', file);
  },
};

module.exports = {
  commands: [financesCommands],
  elements: {
    firstSwitchInput: {
      selector: '#switch0',
    },
    secondSwitchInput: {
      selector: '#switch1',
    },
    firstButton: {
      selector: 'button[data-testid=button0]',
    },
    secondButton: {
      selector: 'button[data-testid=button1]',
    },
    thirdButton: {
      selector: 'button[data-testid=button2]',
    },
    fourthButton: {
      selector: 'button[data-testid=button3]',
    },
    //Form
    netSocial: {
      selector: 'input[data-testid=netSocial]',
    },
    interestDividends: {
      selector: 'input[data-testid=interestDividends]',
    },
    retirementPension: {
      selector: 'input[data-testid=retirementPension]',
    },
    iraTaxableAmount: {
      selector: 'input[data-testid=iraTaxableAmount]',
    },
    wages: {
      selector: 'input[data-testid=wages]',
    },
    capitalGains: {
      selector: 'input[data-testid=capitalGains]',
    },
    businessIncome: {
      selector: 'input[data-testid=businessIncome]',
    },
    otherIncome: {
      selector: 'input[data-testid=otherIncome]',
    },
    rentalIncome: {
      selector: 'input[data-testid=rentalIncome]',
    },
    gifts: {
      selector: 'input[data-testid=gifts]',
    },
    otherCountriesIncome: {
      selector: 'input[data-testid=otherCountriesIncome]',
    },
    unemployment: {
      selector: 'input[data-testid=unemployment]',
    },
    publicAssistance: {
      selector: 'input[data-testid=publicAssistance]',
    },
    gamblingIncome: {
      selector: 'input[data-testid=gamblingIncome]',
    },
    disabilityIncome: {
      selector: 'input[data-testid=disabilityIncome]',
    },
    serviceRelatedDisabilityIncome: {
      selector: 'input[data-testid=serviceRelatedDisabilityIncome]',
    },
    trustRoyaltyIncome: {
      selector: 'input[data-testid=trustRoyaltyIncome]',
    },
    bonds: {
      selector: 'input[data-testid=bonds]',
    },
    alimony: {
      selector: 'input[data-testid=alimony]',
    },
    disabilitySourceSelect: {
      selector: 'input[name=f2020_DisabilitySource]',
    },
    uploadFile: {
      selector: '#form2020UploadFile',
    },
    uploadFileImage: {
      selector: '#form2020UploadFileUploadedImage0',
    },
    uploadFileImageName: {
      selector: '#form2020UploadFileFileName0',
    },
    uploadFileIsValid: {
      selector: '#form2020UploadFileIsValidImage0',
    },

    uploadFileImage2: {
      selector: '#form2020UploadFileUploadedImage1',
    },
    uploadFileImageName2: {
      selector: '#form2020UploadFileFileName1',
    },
    uploadFileIsValid2: {
      selector: '#form2020UploadFileIsValidImage1',
    },

    //Expenses
    assistedLivingInput: {
      selector: 'input[name=assistedLiving]',
    },
    inHomeCareInput: {
      selector: 'input[name=inHomeCare]',
    },
    nonReimbursedPrescriptionsInput: {
      selector: 'input[name=nonReimbursedPrescriptions]',
    },
    approvedMedicareInput: {
      selector: 'input[name=approvedMedicare]',
    },
    medicareProviderSelect: {
      selector: '#mui-component-select-medicareProvider',
    },
    medicareProviderSelectInput: {
      selector: 'input[name=medicareProvider]',
    },
    mediaPlanSelect: {
      selector: '#mui-component-select-medicalPlan',
    },
    mediaPlanSelectInput: {
      selector: 'input[name=medicalPlan]',
    },
    yearExpenseFileInput: {
      selector: '#expenses1099',
    },
    yearExpenseFileInputImage: {
      selector: '#expenses1099UploadedImage0',
    },
    yearExpenseFileInputImageName: {
      selector: '#expenses1099FileName0',
    },
    yearExpenseFileInputIsValid: {
      selector: '#expenses1099IsValidImage0',
    },

    yearExpenseFileInputImage2: {
      selector: '#expenses1099UploadedImage1',
    },
    yearExpenseFileInputImageName2: {
      selector: '#expenses1099FileName1',
    },
    yearExpenseFileInputIsValid2: {
      selector: '#expenses1099IsValidImage1',
    },

    //General
    continueButton: {
      selector: 'button[data-testid=formContinue]',
    },
    continueMain: {
      selector: 'button[data-testid=mainContinue]',
    },
  },
};

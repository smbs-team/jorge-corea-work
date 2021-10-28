module.exports = {
  elements: {
    //name
    firstNameInput: {
      selector: 'input[name=firstName]',
    },
    middleNameInput: {
      selector: 'input[name=middleName]',
    },
    lastNameInput: {
      selector: 'input[name=lastFamilyName]',
    },
    suffixSelect: {
      selector: '#mui-component-select-suffix',
    },
    suffixInput: {
      selector: 'input[name=suffix]',
    },
    //age
    dateOfBirthInput: {
      selector: 'input[name=dateOfBirthInput]',
    },
    myageUploadFileInput: {
      selector: '#myageUploadFile',
    },
    myAgeUploadFileInputImage: {
      selector: '#myageUploadFileUploadedImage0',
    },
    myAgeUploadFileIsValidImage: {
      selector: '#myageUploadFileIsValidImage0',
    },
    myAgeUploadFileImageName: {
      selector: '#myageUploadFileFileName0',
    },
    //redaction tool
    redactionToolContinueButton: {
      selector: 'button[data-testid=continue]',
    },
    //contact info
    mailInput: {
      selector: 'input[name=emailAddress]',
    },
    phoneNumerInput: {
      selector: 'input[name=phoneNumber]',
    },
    smsSwitch: {
      selector: '#textSmsCapable',
    },
    //additional info
    addSpouseSwitch: {
      selector: '#addSpouse',
    },
    //spouse info
    spouseFirstNameInput: {
      selector: 'input[name=spouseFirstName]',
    },
    spouseMiddleNameInput: {
      selector: 'input[name=spouseMiddleName]',
    },
    spouseLastFamilyNameInput: {
      selector: 'input[name=spouseLastFamilyName]',
    },
    spouseSuffixSelect: {
      selector: '#mui-component-select-spouseSuffix',
    },
    spouseSuffixInput: {
      selector: 'input[name=spouseSuffix]',
    },
    spouseDateOfBirthInput: {
      selector: 'input[name=spouseDateOfBirthInput]',
    },
    spouseProofAgeFileInput: {
      selector: '#spouseProofAge',
    },
    spouseProofAgeFileInputImage: {
      selector: '#spouseProofAgeUploadedImage0',
    },
    spouseProofAgeFileInputIsValid: {
      selector: '#spouseProofAgeIsValidImage0',
    },
    spouseProofAgeFileInputImageName: {
      selector: '#spouseProofAgeFileName0',
    },
    veteranSwitch: {
      selector: '#veteranSpouse',
    },
    continueButton: {
      selector: '//div[@class="continue-summary-panel"]/button',
      locateStrategy: 'xpath',
    },
    finances: {
      selector: '#full-width-tab-2',
    },
    exemptionYearRadio2016: {
      selector: 'label[data-testid=radio1]',
    },
    exemptionYearRadioInput: {
      selector: '//label[@data-testid="radio1"]/span',
      locateStrategy: 'xpath',
    },
  },
};

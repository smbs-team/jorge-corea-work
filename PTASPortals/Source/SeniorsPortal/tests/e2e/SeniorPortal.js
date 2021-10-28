//var dynamics = require('../../src/services/test.js');
var data = require('../testFiles/Data.js');

module.exports = {
  before: function(browser) {},

  'End to end Senior Portal': function(browser) {
    var introPage = browser.page.Intro();
    var signInPage = browser.page.SignIn();
    var myInfoPage = browser.page.MyInfo();
    var propertyPage = browser.page.PropertyInfo();
    var financesPage = browser.page.Finances();
    var shared = browser.page.Shared();
    var summaryPage = browser.page.Summary();
    var { ENTER, SPACE, TAB, ARROW_DOWN } = browser.Keys;
    var {
      MY_INFO,
      PROPERTY,
      EXPENSES,
      EXPENSES2,
      SIGN_IN,
      TEST,
      SUFFIX,
      EXPENSES_DATA,
    } = data;
    var { FORM, FORM2 } = data.FINANCES;

    introPage
      .navigate()
      .waitForElementVisible('body')
      .sendKeys('@startAppButton', ENTER);

    signInPage
      //.signInGoogle(MY_INFO.EMAIL, MY_INFO.PASSWORD, SIGN_IN.GOOGLE_LOGIN)
      .signInOutlook(MY_INFO.EMAIL, MY_INFO.PASSWORD, SIGN_IN.OUTLOOK_LOGIN)
      .pause(2000);

    browser.windowHandles(function(result) {
      var handle = result.value[1];
      browser.switchWindow(handle);
    });

    // myInfoPage
    //   .waitForElementVisible('@firstNameInput', 30000)
    //   .setValue('@firstNameInput', MY_INFO.FIRST_NAME)
    //   .setValue('@middleNameInput', MY_INFO.MIDDLE_NAME)
    //   .setValue('@lastNameInput', MY_INFO.LAST_NAME)
    //   .sendKeys('@suffixInput', ENTER)
    //   .pause(1000)
    //   .advClick(SUFFIX.JR)
    //   .pause(1000)
    //   .setValue('@dateOfBirthInput', MY_INFO.DATE_OF_BIRTH)
    //   .uploadImage('@myageUploadFileInput', 'myInfo_proofAge.png')

    //   .click('@exemptionYearRadio2016')

    //   .setValue('@phoneNumerInput', MY_INFO.PHONE)
    //   .sendKeys('@smsSwitch', SPACE)

    //   .sendKeys('@addSpouseSwitch', SPACE)
    //   .pause(1000)
    //   .setValue('@spouseFirstNameInput', MY_INFO.PARTNER.FIRST_NAME)
    //   .setValue('@spouseMiddleNameInput', MY_INFO.PARTNER.MIDDLE_NAME)
    //   .setValue('@spouseLastFamilyNameInput', MY_INFO.PARTNER.LAST_NAME)
    //   .sendKeys('@spouseSuffixInput', ENTER)
    //   .pause(1500)
    //   .advClick(SUFFIX.JR)
    //   .pause(2000)

    //   .sendKeys('@veteranSwitch', SPACE)
    //   .sendKeys('@continueButton', ENTER)

    // propertyPage
    //   .waitForElementVisible('@propertyInput', 30000)
    //   .setValue('@propertyInput', PROPERTY.MY_ADDRESS)
    //   .pause(5000)
    //   .sendKeys('@propertyInput', ARROW_DOWN)
    //   .pause(300)

    //   .sendKeys('@addDifferentAddress', SPACE)
    //   .pause(500)

    //   .sendKeys('@differentCheckAddressSwitch', SPACE)
    //   .pause(500)
    //   .setValue('@fullNameCheckInput', PROPERTY.CHECK_ADDRESS.FULL_NAME)
    //   .setValue('@checkAdressInput', PROPERTY.CHECK_ADDRESS.ADDRESS)
    //   .pause(5000)
    //   .sendKeys('@checkAdressInput', ARROW_DOWN)
    //   .pause(300)
    //   .setValue('@checkZip', PROPERTY.CHECK_ADDRESS.ZIP)

    //   .setValue('@firstDateAsPrimaryResidenceInput', PROPERTY.FIRST_DATE)
    //   .pause(500)

    //   .sendKeys('@othersLiveInPropertySwitch', SPACE)
    //   .pause(500)
    //   .setValue('@firstNameInput', PROPERTY.PARTNER.FIRST_NAME)
    //   .setValue('@middleNameInput', PROPERTY.PARTNER.MIDDLE_NAME)
    //   .setValue('@lastNameInput', PROPERTY.PARTNER.LAST_NAME)
    //   .click('@suffixInput')
    //   .pause(1000)
    //   .advClick(SUFFIX.SR)
    //   .sendKeys('@livesHereSwitch1', SPACE)
    //   .pause(500)
    //   .sendKeys('@isPropertyCoopSwitch', SPACE)
    //   .pause(500)
    //   .setValue('@nameOfCoopInput', PROPERTY.COOP.NAME)
    //   .setValue('@treasurerInput', PROPERTY.COOP.TREASURER)
    //   .setValue('@treasurerPhoneNumberInput', PROPERTY.COOP.TREASURER_PHONE)
    //   .setValue('@numberOfSharesYouOwnInput', PROPERTY.COOP.NUMBER_OF_SHARES)
    //   .setValue('@totalCoopSharesInput', PROPERTY.COOP.TOTAL_SHARES)
    //   .uploadImage(
    //     '@shareCertificateFileInput',
    //     'propertyInfo_shareCertificate.png'
    //   )

    //   .sendKeys('@ownThroughTrustSwitch', SPACE)
    //   .pause(1000)
    //   .uploadImage('@trustDocumentFileInput', 'propertyInfo_trustDoc.png')

    //   .sendKeys('@ownOtherPropertiesSwitch', SPACE)
    //   .pause(1000)
    //   .setValue('@propertyAddressInput', PROPERTY.OTHER_ADDRESS1)
    //   .pause(5000)
    //   .sendKeys('@propertyAddressInput', ARROW_DOWN)
    //   .pause(300)
    //   .sendKeys('@propertyPurposeInput', ENTER)
    //   .pause(500)
    //   .advClick(PROPERTY.OTHER_PURPOSE.RENTAL)

    //   .sendKeys('@isTransferringExemptionSwitch', SPACE)
    //   .pause(1000)
    //   .setValue('@parcelNumberInput', PROPERTY.PREVIOUS_PARCEL_NUM)
    //   .pause(500)
    //   .setValue('@previousExceptionAddressInput', PROPERTY.PREVIOUS_ADDRESS)
    //   .pause(5000)
    //   .sendKeys('@previousExceptionAddressInput', ARROW_DOWN)
    //   .pause(1000)
    //   .setValue('@exemptionAdressZipInput', PROPERTY.PREVIOUS_ZIP)
    //   .sendKeys('@isDelinquentSwitch', SPACE)
    //   .sendKeys('@continueButton', ENTER)

    // financesPage
    //   .waitForElementVisible('@firstButton', 30000)
    //   .sendKeys('@firstButton', ENTER)
    //   .addFinancialInfo(FORM, 'finances_income2019.png')
    //   .addExpenses(EXPENSES, 'finances_yearExpenses2019.png')
    //   .waitForElementVisible('@secondButton', 30000)
    //   .sendKeys('@secondButton', ENTER)
    //   .addFinancialInfo(FORM2, 'finances_income1099.png')
    //   .addExpenses(EXPENSES2, 'finances_yearExpenses1040A.png')
    //   .waitForElementVisible('@firstButton', 30000)

    // shared.sendKeys('@myInfo', ENTER);

    //assert myInfo
    myInfoPage
      .waitForElementVisible('@firstNameInput', 30000)
      .assert.value('@firstNameInput', MY_INFO.FIRST_NAME)
      .assert.value('@middleNameInput', MY_INFO.MIDDLE_NAME)
      .assert.value('@lastNameInput', MY_INFO.LAST_NAME)
      .assert.value('@suffixInput', MY_INFO.SUFFIX)

      .assert.value('@dateOfBirthInput', MY_INFO.DATE_OF_BIRTH)
      .assert.attributeContains(
        '@myAgeUploadFileInputImage',
        'src',
        'data:image/png;base64'
      )
      .assert.elementPresent('@myAgeUploadFileIsValidImage')
      .assert.containsText('@myAgeUploadFileImageName', 'myInfo_proofAge.png')
      .assert.attributeContains(
        '@exemptionYearRadioInput',
        'class',
        'Mui-checked'
      )
      .assert.value('@mailInput', MY_INFO.EMAIL)
      .assert.value('@phoneNumerInput', MY_INFO.PHONE_TEST)
      .assert.attributeContains('@smsSwitch', 'data-ischecked', 'true')
      .assert.value('@spouseFirstNameInput', MY_INFO.PARTNER.FIRST_NAME)
      .assert.value('@spouseMiddleNameInput', MY_INFO.PARTNER.MIDDLE_NAME)
      .assert.value('@spouseLastFamilyNameInput', MY_INFO.PARTNER.LAST_NAME)
      .assert.value('@spouseSuffixInput', MY_INFO.PARTNER.SUFFIX)
      .assert.attributeContains('@veteranSwitch', 'data-ischecked', 'true');

    shared.sendKeys('@continueButton', ENTER);

    propertyPage
      .waitForElementVisible('@propertyAddressBox', 30000)
      .assert.containsText('@propertyAddressBox', TEST.PROPERTY.ADDRESS)
      .assert.attributeContains(
        '@addDifferentAddress',
        'data-ischecked',
        'true'
      )

      .assert.value('@mailingAddressFullNameInput', TEST.PROPERTY.FULL_NAME)
      .assert.value('@mailingAddressInput', TEST.PROPERTY.MAILING_ADDRESS)
      .assert.value('@cityInput', TEST.PROPERTY.CITY)
      .assert.value('@stateInput', TEST.PROPERTY.STATE)
      .assert.value('@zipInput', TEST.PROPERTY.ZIP)

      .assert.attributeContains(
        '@differentCheckAddressSwitch',
        'data-ischecked',
        'true'
      )

      .assert.value('@fullNameCheckInput', PROPERTY.CHECK_ADDRESS.FULL_NAME)
      //.assert.value('@checkAdressInput', PROPERTY.CHECK_ADDRESS.MAILING_ADDRESS)
      .assert.value('@checkCity', PROPERTY.CHECK_ADDRESS.CITY)
      .assert.value('@checkState', PROPERTY.CHECK_ADDRESS.STATE)
      .assert.value('@checkZip', PROPERTY.CHECK_ADDRESS.ZIP)

      .assert.attributeContains(
        '@primaryResidenceSwitch',
        'data-ischecked',
        'true'
      )
      .assert.value('@firstDateAsPrimaryResidenceInput', PROPERTY.FIRST_DATE)
      .assert.attributeContains(
        '@liveNineMonthsInput',
        'data-ischecked',
        'true'
      )
      .assert.attributeContains(
        '@othersLiveInPropertySwitch',
        'data-ischecked',
        'true'
      )
      .assert.value('@firstNameInput', PROPERTY.PARTNER.FIRST_NAME)
      .assert.value('@middleNameInput', PROPERTY.PARTNER.MIDDLE_NAME)
      .assert.value('@lastNameInput', PROPERTY.PARTNER.LAST_NAME)
      .assert.value('@suffixInput', PROPERTY.PARTNER.SUFFIX)
      .assert.attributeContains('@livesHereSwitch1', 'data-ischecked', 'true')
      .assert.attributeContains(
        '@isPropertyCoopSwitch',
        'data-ischecked',
        'true'
      )
      .assert.value('@nameOfCoopInput', PROPERTY.COOP.NAME)
      .assert.value('@treasurerInput', PROPERTY.COOP.TREASURER)
      .assert.value(
        '@treasurerPhoneNumberInput',
        PROPERTY.COOP.TREASURER_PHONE_TEST
      )
      .assert.value(
        '@numberOfSharesYouOwnInput',
        PROPERTY.COOP.NUMBER_OF_SHARES
      )
      .assert.value('@totalCoopSharesInput', PROPERTY.COOP.TOTAL_SHARES)
      .assert.attributeContains(
        '@shareCertificateFileInputImage',
        'src',
        'data:image/png;base64'
      )
      .assert.elementPresent('@shareCertificateFileInputIsValid')
      .assert.containsText(
        '@shareCertificateFileInputImageName',
        'propertyInfo_shareCertificate.png'
      )

      .assert.attributeContains(
        '@ownThroughTrustSwitch',
        'data-ischecked',
        'true'
      )

      .assert.attributeContains(
        '@trustDocumentInputImage',
        'src',
        'data:image/png;base64'
      )
      .assert.elementPresent('@trustDocumentFileInputIsValid')
      .assert.containsText(
        '@trustDocumentFileInputImageName',
        'propertyInfo_trustDoc.png'
      )

      .assert.attributeContains(
        '@ownOtherPropertiesSwitch',
        'data-ischecked',
        'true'
      )
      .assert.value('@propertyAddressInput', TEST.PROPERTY.OTHER_ADDRESS)
      .assert.value('@propertyPurposeInput', TEST.PROPERTY.OTHER_PURPOSE.RENTAL)
      .assert.attributeContains(
        '@isTransferringExemptionSwitch',
        'data-ischecked',
        'true'
      )
      // .assert.value(
      //   '@previousExceptionAddressInput',
      //   TEST.PROPERTY.PREVIOUS_ADDRESS
      // )
      .assert.value('@previousCityInput', TEST.PROPERTY.PREVIOUS_CITY)
      .assert.value('@previousStateInput', TEST.PROPERTY.PREVIOUS_STATE)
      .assert.value('@previousZipInput', TEST.PROPERTY.PREVIOUS_ZIP)
      .assert.attributeContains(
        '@isDelinquentSwitch',
        'data-ischecked',
        'true'
      );

    shared.sendKeys('@continueButton', ENTER);

    financesPage
      .waitForElementVisible('@firstButton', 30000)
      .sendKeys('@firstButton', ENTER)
      .pause(4000)
      .assert.value(
        '@netSocial',
        TEST.CURRENCY + FORM.NET_SOCIAL_SECURITY_LESS_MEDIACARE + TEST.DECIMAL
      )
      .assert.value(
        '@interestDividends',
        TEST.CURRENCY + FORM.INTEREST_DIVIDENDS + TEST.DECIMAL
      )
      .assert.value(
        '@retirementPension',
        TEST.CURRENCY + FORM.PENSIONS_AND_ANNUITIES + TEST.DECIMAL
      )
      .assert.value(
        '@iraTaxableAmount',
        TEST.CURRENCY + FORM.IRAS + TEST.DECIMAL
      )
      .assert.value('@wages', TEST.CURRENCY + FORM.WAGES + TEST.DECIMAL)
      .assert.value(
        '@capitalGains',
        TEST.CURRENCY + FORM.CAPITAL_GAINS + TEST.DECIMAL
      )
      .assert.value(
        '@businessIncome',
        TEST.CURRENCY + FORM.BUSINESS_INCOME + TEST.DECIMAL
      )
      .assert.value(
        '@otherIncome',
        TEST.CURRENCY + FORM.OTHER_GAINS + TEST.DECIMAL
      )
      .assert.value('@rentalIncome', TEST.CURRENCY + FORM.RENTAL + TEST.DECIMAL)
      .assert.value('@gifts', TEST.CURRENCY + FORM.GIFTS + TEST.DECIMAL)
      .assert.value(
        '@otherCountriesIncome',
        TEST.CURRENCY + FORM.INCOME_FROM_OTHER_COUNTRIES + TEST.DECIMAL
      )
      .assert.value(
        '@unemployment',
        TEST.CURRENCY + FORM.UNEMPLOYMENT + TEST.DECIMAL
      )
      .assert.value(
        '@publicAssistance',
        TEST.CURRENCY + FORM.PUBLIC_ASSISTANCE + TEST.DECIMAL
      )
      .assert.value(
        '@gamblingIncome',
        TEST.CURRENCY + FORM.GAMBLING + TEST.DECIMAL
      )
      .assert.value(
        '@disabilityIncome',
        TEST.CURRENCY + FORM.DISABILITY_INCOME + TEST.DECIMAL
      )
      .assert.value(
        '@serviceRelatedDisabilityIncome',
        TEST.CURRENCY + FORM.VA_BENEFIT + TEST.DECIMAL
      )
      .assert.value(
        '@trustRoyaltyIncome',
        TEST.CURRENCY + FORM.ROYAL_INCOME + TEST.DECIMAL
      )
      .assert.value(
        '@disabilitySourceSelect',
        FORM.DISABILITY_SOURCE_TEST.SOCIAL_SECURITY
      )
      .assert.value('@bonds', TEST.CURRENCY + FORM.BONDS_INCOME + TEST.DECIMAL)
      .assert.value('@alimony', TEST.CURRENCY + FORM.ALIMONY + TEST.DECIMAL)
      .assert.attributeContains(
        '@uploadFileImage',
        'src',
        'data:image/png;base64'
      )
      .assert.elementPresent('@uploadFileIsValid')
      // .assert.containsText(
      //   '@uploadFileImageName',
      //   'finances_income2019.png'
      // )

      //Expenses
      .assert.value(
        '@assistedLivingInput',
        TEST.CURRENCY + EXPENSES.ASSISTED_LIVING + TEST.DECIMAL
      )
      .assert.value(
        '@inHomeCareInput',
        TEST.CURRENCY + EXPENSES.IN_HOME_CARE + TEST.DECIMAL
      )
      .assert.value(
        '@nonReimbursedPrescriptionsInput',
        TEST.CURRENCY + EXPENSES.NON_REIMBURSED_PRESCRIPTIONS + TEST.DECIMAL
      )
      .assert.value(
        '@approvedMedicareInput',
        TEST.CURRENCY + EXPENSES.APPROVED_MEDICARE + TEST.DECIMAL
      )
      .assert.value('@medicareProviderSelectInput', EXPENSES.MEDIACARE_PROVIDER)
      .assert.value('@mediaPlanSelectInput', EXPENSES.MEDICAL_PLAN)
      .assert.attributeContains(
        '@yearExpenseFileInputImage',
        'src',
        'data:image/png;base64'
      )
      .assert.elementPresent('@yearExpenseFileInputIsValid')
      // .assert.containsText(
      //   '@yearExpenseFileInputImageName',
      //   'finances_yearExpenses2019.png'
      // )
      .sendKeys('@continueButton', ENTER)
      .waitForElementVisible('@secondButton', 30000)
      .sendKeys('@secondButton', ENTER)

      //Finances 2

      .waitForElementVisible('@netSocial')
      .assert.value(
        '@netSocial',
        TEST.CURRENCY + FORM2.NET_SOCIAL_SECURITY_LESS_MEDIACARE + TEST.DECIMAL
      )
      .assert.value(
        '@interestDividends',
        TEST.CURRENCY + FORM2.INTEREST_DIVIDENDS + TEST.DECIMAL
      )
      .assert.value(
        '@retirementPension',
        TEST.CURRENCY + FORM2.PENSIONS_AND_ANNUITIES + TEST.DECIMAL
      )
      .assert.value(
        '@iraTaxableAmount',
        TEST.CURRENCY + FORM2.IRAS + TEST.DECIMAL
      )
      .assert.value('@wages', TEST.CURRENCY + FORM2.WAGES + TEST.DECIMAL)
      .assert.value(
        '@capitalGains',
        TEST.CURRENCY + FORM2.CAPITAL_GAINS + TEST.DECIMAL
      )
      .assert.value(
        '@businessIncome',
        TEST.CURRENCY + FORM2.BUSINESS_INCOME + TEST.DECIMAL
      )
      .assert.value(
        '@otherIncome',
        TEST.CURRENCY + FORM2.OTHER_GAINS + TEST.DECIMAL
      )
      .assert.value(
        '@rentalIncome',
        TEST.CURRENCY + FORM2.RENTAL + TEST.DECIMAL
      )
      .assert.value('@gifts', TEST.CURRENCY + FORM2.GIFTS + TEST.DECIMAL)
      .assert.value(
        '@otherCountriesIncome',
        TEST.CURRENCY + FORM2.INCOME_FROM_OTHER_COUNTRIES + TEST.DECIMAL
      )
      .assert.value(
        '@unemployment',
        TEST.CURRENCY + FORM2.UNEMPLOYMENT + TEST.DECIMAL
      )
      .assert.value(
        '@publicAssistance',
        TEST.CURRENCY + FORM2.PUBLIC_ASSISTANCE + TEST.DECIMAL
      )
      .assert.value(
        '@gamblingIncome',
        TEST.CURRENCY + FORM2.GAMBLING + TEST.DECIMAL
      )
      .assert.value(
        '@disabilityIncome',
        TEST.CURRENCY + FORM2.DISABILITY_INCOME + TEST.DECIMAL
      )
      .assert.value(
        '@serviceRelatedDisabilityIncome',
        TEST.CURRENCY + FORM2.VA_BENEFIT + TEST.DECIMAL
      )
      .assert.value(
        '@trustRoyaltyIncome',
        TEST.CURRENCY + FORM2.ROYAL_INCOME + TEST.DECIMAL
      )
      .assert.value(
        '@disabilitySourceSelect',
        FORM2.DISABILITY_SOURCE_TEST.SOCIAL_SECURITY
      )
      .assert.value('@bonds', TEST.CURRENCY + FORM2.BONDS_INCOME + TEST.DECIMAL)
      .assert.value('@alimony', TEST.CURRENCY + FORM2.ALIMONY + TEST.DECIMAL)
      .assert.attributeContains(
        '@uploadFileImage2',
        'src',
        'data:image/png;base64'
      )
      .assert.elementPresent('@uploadFileIsValid2')
      // .assert.containsText(
      //   '@uploadFileImageName2',
      //   'finances_income1099.png'
      // )

      //Expenses 2
      .assert.value(
        '@assistedLivingInput',
        TEST.CURRENCY + EXPENSES2.ASSISTED_LIVING + TEST.DECIMAL
      )
      .assert.value(
        '@inHomeCareInput',
        TEST.CURRENCY + EXPENSES2.IN_HOME_CARE + TEST.DECIMAL
      )
      .assert.value(
        '@nonReimbursedPrescriptionsInput',
        TEST.CURRENCY + EXPENSES2.NON_REIMBURSED_PRESCRIPTIONS + TEST.DECIMAL
      )
      .assert.value(
        '@approvedMedicareInput',
        TEST.CURRENCY + EXPENSES2.APPROVED_MEDICARE + TEST.DECIMAL
      )
      .assert.value(
        '@medicareProviderSelectInput',
        EXPENSES2.MEDIACARE_PROVIDER
      )
      .assert.value('@mediaPlanSelectInput', EXPENSES2.MEDICAL_PLAN)
      .assert.attributeContains(
        '@yearExpenseFileInputImage2',
        'src',
        'data:image/png;base64'
      )
      .assert.elementPresent('@yearExpenseFileInputIsValid2')
      // .assert.containsText(
      //   '@yearExpenseFileInputImageName2',
      //   'finances_yearExpenses1040A.png'
      // )

      .sendKeys('@continueButton', ENTER)
      .waitForElementVisible('@continueMain', 30000)
      .sendKeys('@continueMain', ENTER);

    summaryPage
      .waitForElementVisible('@iframe', 30000)
      .assert.elementPresent('@iframe')
      .sendKeys('@signApp', ENTER)
      .pause(2000)
      .setValue(
        '@fullNameInput',
        MY_INFO.FIRST_NAME + ' ' + MY_INFO.MIDDLE_NAME + ' ' + MY_INFO.LAST_NAME
      )
      .assert.value(
        '@fullNameInput',
        MY_INFO.FIRST_NAME + ' ' + MY_INFO.MIDDLE_NAME + ' ' + MY_INFO.LAST_NAME
      )
      .setValue('@witness1Input', MY_INFO.WITNESS1)
      .assert.value('@witness1Input', MY_INFO.WITNESS1)
      .setValue('@witness2Input', MY_INFO.WITNESS2)
      .assert.value('@witness2Input', MY_INFO.WITNESS2)
      .sendKeys('@signFinalButton', ENTER)
      .waitForElementVisible('@appFiledBox', 60000)
      .assert.elementPresent('@appFiledBox');

    browser.end();
  },
};

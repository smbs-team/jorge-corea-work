var signInCommands = {
  signInGoogle: function(email, password, url) {
    var { ENTER } = this.api.Keys;
    return this.waitForElementVisible('@emailInput')
      .setValue('@emailInput', email)
      .sendKeys('@continueButton', ENTER)
      .waitForElementVisible('@userMessageText')
      .navigate(url)
      .waitForElementVisible('@googleEmailInput')
      .setValue('@googleEmailInput', email)
      .sendKeys('@googleNextButton', ENTER)
      .pause(1000)
      .waitForElementVisible('@googlePasswordInput')
      .setValue('@googlePasswordInput', password)
      .sendKeys('@googlePasswordNextButton', ENTER)
      .waitForElementVisible('@googleConfirmationEmail', 30000)
      .click('@googleConfirmationEmail')
      .pause(1000)
      .click('@emailConfirmationLink');
  },
  signInOutlook: function(email, password, url) {
    var { ENTER } = this.api.Keys;
    return this.waitForElementVisible('@emailInput')
      .setValue('@emailInput', email)
      .sendKeys('@continueButton', ENTER)
      .waitForElementVisible('@userMessageText')
      .navigate(url)
      .sendKeys('@loginButtonOutlook', ENTER)
      .waitForElementVisible('@emailOutlookInput')
      .setValue('@emailOutlookInput', email)
      .sendKeys('@submitEmailOutlook', ENTER)
      .waitForElementVisible('@passOutlookInput')
      .setValue('@passOutlookInput', password)
      .sendKeys('@submitEmailOutlook', ENTER)
      .waitForElementVisible('@outlookConfirmationEmail', 30000)
      .click('@outlookConfirmationEmail')
      .pause(1000)
      .advClick("div[class='x_button']");
  },
};

module.exports = {
  commands: [signInCommands],
  elements: {
    loginButtonOutlook: {
      selector: 'a[data-task=signin]',
    },
    emailOutlookInput: {
      selector: 'input[type=email]',
    },
    submitEmailOutlook: {
      selector: 'input[type=submit]',
    },
    passOutlookInput: {
      selector: 'input[name=passwd]',
    },
    emailInput: {
      selector: '#email',
    },
    continueButton: {
      selector: '#continue',
    },
    userMessageText: {
      selector: '#userMessage',
    },
    googleEmailInput: {
      selector: '#identifierId',
    },
    googleNextButton: {
      selector: '#identifierNext',
    },
    googlePasswordInput: {
      selector: 'input[name=password]',
    },
    googlePasswordNextButton: {
      selector: '#passwordNext',
    },
    googleEmailsTable: {
      selector: 'table[role=grid]',
    },
    googleConfirmationEmail: {
      selector: '//table[@role="grid"]/tbody/tr[1]',
      locateStrategy: 'xpath',
    },
    emailConfirmationLink: {
      selector: '//a[contains(@href, "https://kcb2csandbox.b2clogin.com/")]',
      locateStrategy: 'xpath',
    },
    outlookConfirmationEmail: {
      selector: '//div[contains(@aria-label, "King County")][1]',
      locateStrategy: 'xpath',
    },
  },
};

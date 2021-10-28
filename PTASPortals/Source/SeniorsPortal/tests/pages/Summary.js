module.exports = {
  elements: {
    signApp: {
      selector: 'button[data-testid=signApp]',
    },
    iframe: {
      selector: '#summary',
    },
    myInfoFullName: {
      selector: '//table/tbody/tr/th',
      locateStrategy: 'xpath',
    },
    fullNameInput: {
      selector: 'input[data-testid=fullName]',
    },
    witness1Input: {
      selector: 'input[data-testid=witness1]',
    },
    witness2Input: {
      selector: 'input[data-testid=witness2]',
    },
    signFinalButton: {
      selector: 'button[data-testid=signFinal]',
    },
    finalDialog: {
      selector: 'div[role=dialog]',
    },
    goHomeButton: {
      selector: 'button[data-testid=goHome]',
    },
    appFiledBox: {
      selector: 'div[data-testid=appFiledBox]',
    },
  },
};

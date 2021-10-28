module.exports = {
  url: 'https://localhost:3000/intro',
  elements: {
    dateInput: {
      selector: 'input[type=text]',
    },
    incomeIput: {
      selector: 'input[name=householdIncome]',
    },
    startAppButton: {
      selector: 'button[data-testid=signIn]',
    },
    signInMicrosoftButton: {
      selector: '#MicrosoftAccountExchange',
    },
  },
};

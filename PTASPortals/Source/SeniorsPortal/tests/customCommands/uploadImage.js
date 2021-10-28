exports.command = function(selector, fileName) {
  return this.setValue(
    selector,
    require('path').resolve('./tests/testFiles/' + fileName)
  )
    .pause(2000)
    .sendKeys('button[data-testid=continue]', this.Keys.ENTER)
    .pause(2000);
};

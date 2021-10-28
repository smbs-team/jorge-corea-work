exports.command = function(query) {
  return this.execute(
    function(query) {
      document.querySelector(query).click();
    },
    [query]
  );
};

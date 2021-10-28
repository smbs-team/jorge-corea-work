var glob = require("glob"),
  path = require("path"),
  fs = require("fs");

renameBundleJS();
renameBundleJSMap();
renameBundleCSS();
renameBundleCSSMap();

function renameBundleJS() {
  glob(__dirname + "/../../build/static/js/*.js", function(err, files) {
    files.forEach(function(file) {
      // Change reference to map file
      var data = fs.readFileSync(file, "utf8");
      var result = data.replace(
        /# sourceMappingURL=.*$/g,
        "# sourceMappingURL=bundle.js.map"
      );
      fs.writeFileSync(file, result, "utf8");

      // rename file
      var dir = path.dirname(file);
      var filename = path.basename(file);
      fs.renameSync(file, dir + "/kc_redationtool_static_js_bundle.js");
    });
  });
}

function renameBundleJSMap() {
  glob(__dirname + "/../../build/static/js/*.js.map", function(err, files) {
    files.forEach(function(file) {
      // rename map file
      var dir = path.dirname(file);
      var filename = path.basename(file);
      fs.renameSync(file, dir + "/kc_redationtool_static_js_bundle.js.map.js");
    });
  });
}

function renameBundleCSS() {
  glob(__dirname + "/../../build/static/css/*.css", function(err, files) {
    files.forEach(function(file) {
      // change reference to map file and also replace reference to fonts
      // file that we had to rename with _ instead of - in the web resource.
      var data = fs.readFileSync(file, "utf8");
      var result = data.replace(
        /# sourceMappingURL=.*$/g,
        "# sourceMappingURL=styles.css.map*/"
      );
      result = result.replace(
        /baseline-arrow_drop_down_circle-28px/g,
        "baseline_arrow_drop_down_circle_28px"
      );
      result = result.replace(/round-cancel-24px/g, "round_cancel_24px");
      result = result.replace(
        /status-change-example/g,
        "status_change_example"
      );
      fs.writeFileSync(file, result, "utf8");

      // rename file
      var dir = path.dirname(file);
      var filename = path.basename(file);
      fs.renameSync(file, dir + "/kc_redationtool_static_css_styles.css");
    });
  });
}

function renameBundleCSSMap() {
  glob(__dirname + "/../../build/static/css/*.css.map", function(err, files) {
    files.forEach(function(file) {
      // rename css file
      var dir = path.dirname(file);
      var filename = path.basename(file);
      fs.renameSync(
        file,
        dir + "/kc_redationtool_static_css_styles.css.map.css"
      );
    });
  });
}

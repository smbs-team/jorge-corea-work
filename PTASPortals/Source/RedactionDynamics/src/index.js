import React from "react";
import ReactDOM from "react-dom";
import "./index.css";
import App from "./App";
import * as serviceWorker from "./serviceWorker";
import "@formatjs/intl-pluralrules/polyfill";
import locale_en from "@formatjs/intl-pluralrules/dist/locale-data/en";

new Intl.PluralRules(locale_en).select(0);

// Hide log messages from prod environment
if (process.env.NODE_ENV !== "development") {
  console.log = () => {};
}

ReactDOM.render(<App />, document.getElementById("root"));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();

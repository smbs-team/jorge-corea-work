import React from "react";
import { ThemeProvider } from "@material-ui/core";
import {
  ptasCamaTheme,
  ErrorMessageProvider
} from "@ptas/react-public-ui-library";
// no need to use the correct name of the component
import Component from "./Examples/CustomSearchTextFields";
import { IntlProvider } from "react-intl";
import Spanish from "./Examples/es-lang.json";
import English from "./Examples/en-lang.json";

const local = navigator.language;
let lang;
if (local === "en-US") {
  lang = English;
} else {
  lang = Spanish;
}

const App = () => {
  return (
    <ThemeProvider theme={ptasCamaTheme}>
      <IntlProvider locale={"en"} messages={lang}>
        <ErrorMessageProvider>
          <Component />
        </ErrorMessageProvider>
      </IntlProvider>
    </ThemeProvider>
  );
};

export default App;

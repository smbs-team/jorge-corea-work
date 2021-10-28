import React from "react";
import { ThemeProvider } from "@material-ui/core";
import { ptasCamaTheme } from "@ptas/react-ui-library";
// no need to use the correct name of the component
import Component from "./Examples/AutoComplete";
import "@ptas/react-ui-library/dist/index.css";

const App = () => {
  return (
    <ThemeProvider theme={ptasCamaTheme}>
      <Component />
    </ThemeProvider>
  );
};

export default App;

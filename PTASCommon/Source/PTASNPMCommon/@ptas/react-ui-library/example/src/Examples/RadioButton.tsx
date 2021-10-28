import React from "react";
import { ThemeProvider } from "@material-ui/core";
import { RadioButtonGroup, ptasCamaTheme } from "@ptas/react-ui-library";

const RadioButton = () => {
  return (
    <ThemeProvider theme={ptasCamaTheme}>
      <RadioButtonGroup
        items={[
          { value: "one", label: "The one label" },
          { value: "two", label: "Label of steel" }
        ]}
        onChange={(value) => console.log(value)}
      ></RadioButtonGroup>
    </ThemeProvider>
  );
};

export default RadioButton;

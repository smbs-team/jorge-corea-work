import React from "react";
import { CustomNumericField as Numeric } from "@ptas/react-ui-library";


const CustomNumericField = () => {
  return (
    <Numeric
      label='Numeric Field'
      onValueChange={(e) => {
        console.log(e);
        //returns Object { formattedValue: "", value: "", floatValue: "" }
      }}
      minValue={0}
      maxValue={20000}
      decimalScale={2}
      NumericProps={{ thousandSeparator: true }}
    />
  );
};

export default CustomNumericField;

import React from "react";
import CustomTextField from "../CustomTextField";
import NumberFormat, {
  NumberFormatProps,
  NumberFormatValues
} from "react-number-format";
import { TextFieldProps } from "@material-ui/core";
import { omit } from "lodash";

interface Props {
  maxValue?: number;
  minValue?: number;
  decimalScale?: number;
  NumericProps?: NumberFormatProps;
  onValueChange?: (values: NumberFormatValues) => void;
}

function CustomNumericField(props: Props & Omit<TextFieldProps, "onChange">) {
  const allowedNumberRange = (values: NumberFormatValues): boolean => {
    const { formattedValue, floatValue } = values;
    if (floatValue == null) {
      return formattedValue === "";
    } else {
      return (
        floatValue <= (props.maxValue ?? Number.MAX_VALUE) &&
        floatValue >= (props.minValue ?? Number.MIN_VALUE)
      );
    }
  };

  const NumberFormatCustom = (inputProps: any): JSX.Element => {
    const { inputRef, onChange, ...other } = inputProps;
    return (
      <NumberFormat
        {...other}
        getInputRef={inputRef}
        decimalScale={props.decimalScale ?? 0}
        onValueChange={(values) => {
          onChange(values);
          props.onValueChange && props.onValueChange(values);
        }}
        isAllowed={allowedNumberRange}
        {...props.NumericProps}
      />
    );
  };

  const textFieldProps = omit(props, [
    "maxValue",
    "minValue",
    "decimalScale",
    "NumericProps",
    "onValueChange"
  ]);

  return (
    <CustomTextField
      InputProps={{
        inputComponent: NumberFormatCustom,
        ...textFieldProps.InputProps
      }}
      {...omit(textFieldProps, "InputProps")}
    />
  );
}

export default CustomNumericField;

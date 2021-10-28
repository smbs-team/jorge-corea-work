import React from "react";
import {
  CustomTextField,
  SimpleDropDown,
  CustomDatePicker,
  CustomPhoneTextField,
  CustomTextarea,
  CustomButton
} from "@ptas/react-public-ui-library";
import { makeStyles } from "@material-ui/core";
import { useState } from "react";

const useStyles = makeStyles(() => ({
  outlineTextField: {
    // width: 40,
    // marginRight: 50
  }
}));

const CustomButtonComp = () => {
  const classes = useStyles();
  const [testInputError, setTestInputError] = useState<boolean>(false);

  return (
    <div style={{ display: "flex", flexWrap: "wrap" }}>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomTextField
          ptasVariant='underline'
          label='Underline'
          secondaryLabel='NIO'
          onChange={(e) => console.log(e.target.value)}
          readOnly
        />
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomTextField
          classes={{ root: classes.outlineTextField }}
          ptasVariant='outline'
          label='Outline'
        />
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomTextField
          ptasVariant='overlay'
          label='Overlay'
          secondaryLabel='NIO'
          helperText='helper text example'
        />
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomTextField ptasVariant='currency' label='Currency' showIcon />
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <SimpleDropDown
          items={[
            { value: "one", label: "country" },
            { value: "two", label: "text large" }
          ]}
          onSelected={(value) => console.log(value)}
          label='Menu of steel'
        ></SimpleDropDown>
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomDatePicker label='Date of destruction' onChangeDelay={1} />
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomPhoneTextField label='phone' placeholder='' />
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomTextField variant='filled' type='email' label='email' />
      </div>
      <div
        style={{
          marginRight: 5,
          marginLeft: 5,
          padding: 10
        }}
      >
        <CustomTextarea
          onChange={(e) => console.log("textarea value", e.currentTarget.value)}
          rows={5}
          label='Hola'
          helperText='example helper'
        />
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomTextField
          ptasVariant='overlay'
          label='Test input'
          helperText='helper text example'
          // InputProps={{

          // }}
          error={testInputError}
          onChange={(e) => {
            if (!e.currentTarget.value) {
              setTestInputError(true);
            } else if (testInputError && e.currentTarget.value) {
              setTestInputError(false);
            }
          }}
          onBlur={() => console.log("on blur")}
        />
        <CustomButton
          disabled={testInputError}
          onClick={(): void => {
            console.log("click");
          }}
          ptasVariant='Primary'
        >
          Continue
        </CustomButton>
      </div>
    </div>
  );
};

export default CustomButtonComp;

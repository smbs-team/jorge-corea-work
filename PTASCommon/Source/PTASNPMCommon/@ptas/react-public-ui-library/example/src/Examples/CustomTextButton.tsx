import React from "react";
import { CustomTextButton } from "@ptas/react-public-ui-library";

const CustomButtonComp = () => {
  return (
    <div style={{ display: "flex", flexWrap: "wrap" }}>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomTextButton ptasVariant='Clear'>Clear</CustomTextButton>
      </div>
      <div style={{ marginRight: 5, marginLeft: 5, background: "black" }}>
        <CustomTextButton ptasVariant='Inverse clear' disabled>
          Inverse clear
        </CustomTextButton>
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomTextButton ptasVariant='Text large'>Text Large</CustomTextButton>
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomTextButton ptasVariant='Text'>Text</CustomTextButton>
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomTextButton ptasVariant='Text more'>Text More</CustomTextButton>
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomTextButton ptasVariant='Text more small'>
          Text More Small
        </CustomTextButton>
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomTextButton ptasVariant='Text more small min'>
          Text More Small Min
        </CustomTextButton>
      </div>
      <div style={{ marginRight: 5, marginLeft: 5, background: "black" }}>
        <CustomTextButton ptasVariant='Inverse more blue'>
          Inverse More blue
        </CustomTextButton>
      </div>
      <div style={{ marginRight: 5, marginLeft: 5, background: "black" }}>
        <CustomTextButton ptasVariant='Inverse more'>
          Inverse More
        </CustomTextButton>
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomTextButton disabled ptasVariant='Danger more'>
          Danger More
        </CustomTextButton>
      </div>
    </div>
  );
};

export default CustomButtonComp;

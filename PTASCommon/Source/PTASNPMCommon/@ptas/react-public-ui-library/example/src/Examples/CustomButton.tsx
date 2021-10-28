import React from "react";
import { CustomButton } from "@ptas/react-public-ui-library";

const CustomButtonComp = () => {
  return (
    <div style={{ display: "flex", flexWrap: "wrap" }}>
      <div style={{ marginLeft: "5px", marginRight: "5px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>Primary</span>
        <CustomButton
          onClick={() => {
            console.log("was clicked");
          }}
          ptasVariant='Primary'
        >
          Save
        </CustomButton>
      </div>
      <div style={{ marginLeft: "5px", marginRight: "5px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>Secondary</span>
        <CustomButton ptasVariant='Secondary'>Save</CustomButton>
      </div>
      <div style={{ marginLeft: "5px", marginRight: "5px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>Outline</span>
        <CustomButton ptasVariant='Outline'>Save</CustomButton>
      </div>
      <div style={{ marginLeft: "5px", marginRight: "5px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>Inverse</span>
        <CustomButton ptasVariant='Inverse'>Save</CustomButton>
      </div>
      <div style={{ marginLeft: "5px", marginRight: "5px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>
          Danger Inverse
        </span>
        <CustomButton ptasVariant='Danger inverse'>Save</CustomButton>
      </div>
      <div style={{ marginLeft: "5px", marginRight: "5px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>Danger</span>
        <CustomButton ptasVariant='Danger'>Save</CustomButton>
      </div>
      <div style={{ marginLeft: "5px", marginRight: "5px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>Roundish</span>
        <CustomButton ptasVariant='Roundish'>Save</CustomButton>
      </div>
      <div style={{ marginLeft: "5px", marginRight: "5px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>Sharp</span>
        <CustomButton ptasVariant='Sharp'>Save</CustomButton>
      </div>
      <div style={{ marginLeft: "5px", marginRight: "5px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>Large</span>
        <CustomButton ptasVariant='Large'>Save</CustomButton>
      </div>
      <div style={{ marginLeft: "5px", marginRight: "5px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>Short</span>
        <CustomButton ptasVariant='Short'>Save</CustomButton>
      </div>
      <div style={{ marginLeft: "5px", marginRight: "5px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>Slim</span>
        <CustomButton ptasVariant='Slim'>Save</CustomButton>
      </div>
      <div style={{ marginLeft: "5px", marginRight: "5px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>
          Slim Outline
        </span>
        <CustomButton ptasVariant='Slim outline'>Save</CustomButton>
      </div>
      <div style={{ marginLeft: "5px", marginRight: "5px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>
          Inverse Slim
        </span>
        <CustomButton ptasVariant='Inverse slim' disabled>
          Save
        </CustomButton>
      </div>
    </div>
  );
};

export default CustomButtonComp;

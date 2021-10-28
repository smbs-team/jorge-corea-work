import React from "react";
import { CustomSwitch, CustomDualSwitch } from "@ptas/react-public-ui-library";

const CustomButtonComp = () => {
  const Component = (
    <span>
      Label as <strong style={{ display: "block" }}>Component</strong>
    </span>
  );
  return (
    <div style={{ display: "flex", flexWrap: "wrap" }}>
      <div style={{ marginLeft: "5px", marginRight: "5px" }}>
        <CustomSwitch label={Component} ptasVariant='small' showOptions />
      </div>
      <div style={{ marginLeft: "5px", marginRight: "5px" }}>
        <CustomSwitch label={Component} ptasVariant='normal' showOptions />
      </div>
      <div style={{ marginLeft: "5px", marginRight: "5px" }}>
        <CustomDualSwitch leftLabel='Label1' rigthLabel='Label2' />
      </div>
    </div>
  );
};

export default CustomButtonComp;

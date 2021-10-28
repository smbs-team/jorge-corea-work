import React from "react";
import { CustomRange } from "@ptas/react-public-ui-library";

const CustomPopoverExample = () => {
  return (
    <div style={{ display: "flex", flexWrap: "wrap" }}>
      <div style={{ margin: "0 auto" }}>
        <CustomRange max={100} min={0} defaultValue={0} label='AA' />
      </div>
    </div>
  );
};

export default CustomPopoverExample;

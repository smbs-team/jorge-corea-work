import React from "react";
import { RadioButtonGroup } from "@ptas/react-public-ui-library";
const RadioButtons = () => {
  return (
    <div style={{ display: "flex", flexWrap: "wrap" }}>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <RadioButtonGroup
          onChange={(e) => console.log(e)}
          items={[
            { label: "SI", value: "SI" },
            { label: "NO", value: "no" }
          ]}
          orientation='row'
        />
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <RadioButtonGroup
          onChange={(e) => console.log(e)}
          ptasVariant='normal'
          items={[
            { label: "SI", value: "SI" },
            { label: "NO", value: "no" }
          ]}
          orientation='row'
        />
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <RadioButtonGroup
          ptasVariant='small'
          onChange={(e) => console.log(e)}
          items={[
            { label: "SI", value: "SI" },
            { label: "NO", value: "no" }
          ]}
          orientation='row'
        />
      </div>
    </div>
  );
};
export default RadioButtons;

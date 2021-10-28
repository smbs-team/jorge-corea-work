import React from "react";
import { KeyValuePanel as Panel, SimpleDropDown } from "@ptas/react-ui-library";

const KeyValuePanel = () => {
  return (
    <Panel
      data={[
        {
          key: "hola",
          value: <SimpleDropDown items={[]} onSelected={() => {}} />
        },
        { key: "dos", value: "otro value" }
      ]}
    />
  );
};

export default KeyValuePanel;

import React from "react";
import { Switch as Comp } from "@ptas/react-ui-library";

const Switch = () => {
  return (
    <Comp
      items={["Fill", "Outline"]}
      onSelected={(s) => console.log("selected index", s)}
      selectedIndex={1}
    />
  );
};

export default Switch;

import React from "react";
import { ChipContainer as ChipC } from "@ptas/react-ui-library";

const ChipContainer = () => {
  const chips = [
    { key: 0, label: "Angular" },
    { key: 1, label: "jQuery" },
    { key: 2, label: "Polymer" },
    { key: 3, label: "React" },
    { key: 4, label: "Vue.js" }
  ];

  return <ChipC chipData={chips} onChange={(c) => console.log(c)} />;
};

export default ChipContainer;

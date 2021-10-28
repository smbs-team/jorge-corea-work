import React, { Fragment, useState } from "react";
import {
  ColorConfiguration,
  ColorRampPanel as PtasColorRampPanel
} from "@ptas/react-ui-library";

const ColorRampPanel = () => {
  const [numberOfColorsPerRamp] = useState(22);

  const savedRamp: ColorConfiguration = {
    id: "test",
    colorSet1: ["#00429d", "#96ffea"],
    opacity: 1,
    selectedTab: 0,
    type: "sequential",
    colors: [
      "rgba(0,66,157,1)",
      "rgba(30,78,163,1)",
      "rgba(46,90,168,1)",
      "rgba(59,103,174,1)",
      "rgba(70,116,180,1)",
      "rgba(80,129,185,1)",
      "rgba(89,142,191,1)",
      "rgba(98,156,196,1)",
      "rgba(106,169,202,1)",
      "rgba(114,183,207,1)",
      "rgba(121,197,213,1)",
      "rgba(129,211,218,1)",
      "rgba(136,226,223,1)",
      "rgba(143,240,229,1)",
      "rgba(150,255,234,1)"
    ]
  };

  const randomRamp: ColorConfiguration = {
    id: "738b23d1-3794-4739-a372-2de3ddf66e60",
    colors: [
      "rgba(223,56,106, 1)",
      "rgba(226,156,243, 1)",
      "rgba(30,23,57, 1)"
    ],
    type: "random",
    selectedTab: 0,
    opacity: 1
  }

  
  return (
    <Fragment>
      <PtasColorRampPanel
        ramps={[randomRamp]}
        colorsToGenerate={3}
        selectedFillRampId={"defaultRamp1"}
        // selectedOutlineRampId={"temp_1"}
        title='Automatic color ramp'
        switchContent={["Fill", "Outline"]}
        //opacity={0.12}
        switchOnChange={(s) => {
          console.log(s);
        }}
        onSelect={(val) => {
          console.log("Selected", val);
        }}
        // onEdit={(e) => console.log("Edited ramp", e)}
        // onDelete={(e) => console.log("To delete", e)}
        // onOpacityChange={(o) => console.log("on opacity change", o)}
        //onCreate={(e) => console.log("To create", e)}
      />
      {/* <div>
        <Button
          onClick={() => setNumberOfColorsPerRamp(numberOfColorsPerRamp + 1)}
        >
          Add
        </Button>
      </div> */}
    </Fragment>
  );
};

export default ColorRampPanel;

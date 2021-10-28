import React, { useState, Fragment } from "react";
import { ColorPicker as Color } from "@ptas/react-ui-library";

const ColorPicker = () => {
  const [addColor, setColor] = useState<string>('rgb(105, 189, 210, 0.71)');

  const handleColor = () => {
    setColor('rgb(10, 20, 30)')
    console.log(addColor);
  }

  return (
    <Fragment>
      <Color
        showHexInput
        onChange={(e) => {
          console.log(e);
        }}
        label='Color'
        rgbColor={addColor}
      />
      <br />
      <button onClick={() => handleColor()}>Add Color</button>
      <br />
      <button onClick={() => setColor("rgb(111, 43, 52)")}>Add Different Color</button>
    </Fragment>
  );
};

export default ColorPicker;

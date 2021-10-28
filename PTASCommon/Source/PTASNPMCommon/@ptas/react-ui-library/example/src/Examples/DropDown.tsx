import React, { useState, Fragment } from "react";
import { SimpleDropDown } from "@ptas/react-ui-library";

const DropDown = () => {
  const [value, setValue] = useState<string>();

  return (
    <Fragment>
      <SimpleDropDown
        items={[
          { value: "one", label: "One" },
          { value: "two", label: "Two" }
        ]}
        onSelected={(value) => console.log(value)}
        label='Menu of steel'
        value={value}
      ></SimpleDropDown>
      <button onClick={() => setValue("one")}>Change value to One</button>
      <button onClick={() => setValue("two")}>Change value to Two</button>
    </Fragment>
  );
};

export default DropDown;

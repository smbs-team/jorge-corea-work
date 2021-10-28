import React from "react";
import { Multimedia } from "@ptas/react-public-ui-library";

const CustomChipExample = () => {
  return (
    <div style={{ display: "flex", flexWrap: "wrap" }}>
      <Multimedia
        video='https://vimeo.com/226260195'
        controls
        showTitle={false}
      />
    </div>
  );
};

export default CustomChipExample;

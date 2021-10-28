import React, { Fragment } from "react";
import { ColorLegend } from "@ptas/react-ui-library";

const _ColorLegend = () => {
  const legends = [
    {
      legendText: 'Text',
      fillColor: 'rgba(255,0,0,1)',
      outlineColor: 'rgba(255,0,0,1)',
    }
  ];
  return (
    <Fragment>
      <ColorLegend
        title={"Title"}
        open={true}
        legends={legends}
        // anchorEl={domRef?.current}
        anchorEl={document.body}
        onClose={(): void => {
          //
        }}
        options={{
          placement: "bottom-start"
        }}
      />
    </Fragment>
  );
};

export default _ColorLegend;

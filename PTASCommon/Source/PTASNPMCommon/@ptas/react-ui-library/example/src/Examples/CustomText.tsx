import React, { useState } from "react";
import { CustomTextField } from "@ptas/react-ui-library";

const CustomText = () => {

  return (
      <CustomTextField
        label='Comments'
        onChange={(e): void => console.log(e.target.value)}
      />
  );
};

export default CustomText;

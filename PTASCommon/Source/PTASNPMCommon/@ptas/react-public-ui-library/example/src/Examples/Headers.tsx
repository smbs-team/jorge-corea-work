import React, { Fragment } from "react";
import { Header } from "@ptas/react-public-ui-library";

const FormatExample = () => {
  return (
    <Fragment>
      <div>
        <Header
          ptasVariant='black'
          showDropdown
          showMenuIcon
          showSearchIcon
          defaultValue='es'
        />
      </div>
      <div
        style={{
          padding: 20,
          background: "#0c80a1",
          marginTop: 10,
          marginBottom: 10
        }}
      >
        <Header
          ptasVariant='transparent'
          showDropdown
          showMenuIcon
          showSearchIcon
          dropDownPlaceHolder='Language'
        />
      </div>
      <div>
        <Header ptasVariant='black' />
      </div>
    </Fragment>
  );
};

export default FormatExample;

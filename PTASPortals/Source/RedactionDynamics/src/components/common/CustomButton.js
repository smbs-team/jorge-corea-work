//-----------------------------------------------------------------------
// <copyright file="CustomButton.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import React, { label, style, disabled } from "react";
import "./CustomButton.css";
import Button from "@material-ui/core/Button";
import PropTypes from "prop-types";
import { withStyles } from "@material-ui/styles";
import CircularProgress from "@material-ui/core/CircularProgress";
import { renderIf } from "../../lib/helpers/util";

const styles = theme => ({
  button: {
    "&:disabled": {
      backgroundColor: "#9b6692"
    }
  },
  input: {
    display: "none"
  }
});

const CustomButton = props => {
  return (
    <React.Fragment>
      {renderIf(
        props.isText,
        <span
          style={{ cursor: "pointer" }}
          id={props.id}
          onClick={props.onClick}
        >
          {renderIf(
            props.spinner,
            <CircularProgress className="custom-button-spinner" size={20} />
          )}
          {props.leftIcon ? props.leftIcon : ""}
          {props.label}
        </span>,
        <Button
          id={props.id}
          disabled={props.disabled}
          classes={{
            root: props.secondary
              ? "button btnForms small  interactive  secondary"
              : props.secondaryThin
              ? "button btnFinancialInfo interactive secondary"
              : props.secondaryWhite
              ? "button btnFinancialInfoWhite interactive "
              : "button btnForms small  interactive  primary",

            label: props.btnBigLabel ? "btnLabel" : ""
          }}
          onClick={props.onClick}
          data-testid={props.testid}
        >
          {renderIf(
            props.spinner,
            <CircularProgress className="custom-button-spinner" size={20} />
          )}
          {props.label}
        </Button>
      )}
    </React.Fragment>
  );
};

CustomButton.propTypes = {
  classes: PropTypes.object.isRequired
};

export default withStyles(styles)(CustomButton);

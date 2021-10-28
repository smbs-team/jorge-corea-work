// CustomCheckBox.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, ChangeEvent } from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  Typography,
  Theme
} from "@material-ui/core";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Checkbox, { CheckboxProps } from "@material-ui/core/Checkbox";
import clsx from "clsx";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  onChange: (value: boolean) => void;
  label: string;
  checkBoxProps?: CheckboxProps;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      "&:hover": {
        backgroundColor: "transparent"
      }
    },
    container: {
      width: "fit-content"
    },
    label: {
      fontFamily: "helvetica"
    },
    icon: {
      fontSize: "1.2rem",
      color: theme.ptas.colors.theme.black
    },
    radioIcon: {
      cursor: "pointer",
      borderRadius: "50%",
      width: 24,
      height: 24,
      backgroundColor: theme.ptas.colors.theme.white,
      border: `2px solid ${theme.ptas.colors.theme.black}`
    },
    radioIconChecked: {
      border: `2px solid ${theme.ptas.colors.utility.selection}`,
      "&:before": {
        display: "block",
        width: 20,
        height: 20,
        backgroundImage: `radial-gradient(${theme.ptas.colors.utility.selection}, ${theme.ptas.colors.utility.selection} 60%, transparent 0%)`,
        content: '""'
      }
    }
  });

/**
 * CustomCheckBox
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomCheckBox(props: Props): JSX.Element {
  const { classes, onChange, label } = props;

  const [state, setState] = useState(false);

  const handleChange = (isChecked: boolean) => {
    setState(isChecked);
    onChange(isChecked);
  };

  return (
    <FormControlLabel
      className={classes.container}
      control={
        <Checkbox
          className={classes.root}
          disableRipple
          color='default'
          checked={state}
          onChange={(e: ChangeEvent<HTMLInputElement>) =>
            handleChange(e.target.checked)
          }
          icon={<span className={classes.radioIcon} />}
          checkedIcon={
            <span
              className={clsx(classes.radioIcon, classes.radioIconChecked)}
            />
          }
          {...(props.checkBoxProps || {})}
        />
      }
      label={
        <Typography
          variant='body2'
          color='textPrimary'
          className={classes.label}
        >
          {label}
        </Typography>
      }
    />
  );
}

export default withStyles(useStyles)(CustomCheckBox);

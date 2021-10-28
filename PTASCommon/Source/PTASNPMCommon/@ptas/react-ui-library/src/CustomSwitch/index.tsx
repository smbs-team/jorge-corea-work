// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState } from "react";
import { withStyles, WithStyles, createStyles, Theme } from "@material-ui/core";
import FormGroup from "@material-ui/core/FormGroup";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Switch from "@material-ui/core/Switch";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  label?: string;
  isChecked?: (checked: boolean) => void;
  defaultState?: boolean;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      width: 40,
      height: 20,
      padding: 0,
      margin: theme.spacing(1)
    },
    switchBase: {
      left: 1,
      padding: theme.spacing(0.125),
      "&$checked": {
        transform: "translateX(16px)",
        color: theme.ptas.colors.theme.white,
        "& + $track": {
          backgroundColor: theme.ptas.colors.utility.selection,
          opacity: 1,
          border: "none"
        }
      },
      "&$focusVisible $thumb": {
        color: theme.ptas.colors.utility.selection,
        border: "6px solid #fff"
      }
    },
    thumb: {
      width: 18,
      height: 18
    },
    track: {
      borderRadius: 18,
      backgroundColor: theme.ptas.colors.theme.black,
      opacity: 1,
      transition: theme.transitions.create(["background-color", "border"])
    },
    checked: {
      left: 3
    }
  });

/**
 * CustomSwitch
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomSwitch(props: Props): JSX.Element {
  const { classes, label, isChecked, defaultState } = props;
  const [state, setState] = useState<boolean>(defaultState ?? false);

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setState(event.target.checked);
    isChecked && isChecked(event.target.checked);
  };

  return (
    <FormGroup>
      <FormControlLabel
        control={
          <Switch
            disableRipple
            checked={state}
            onChange={handleChange}
            classes={{
              root: classes.root,
              switchBase: classes.switchBase,
              thumb: classes.thumb,
              track: classes.track,
              checked: classes.checked
            }}
          />
        }
        label={label}
      />
    </FormGroup>
  );
}

export default withStyles(useStyles)(CustomSwitch);

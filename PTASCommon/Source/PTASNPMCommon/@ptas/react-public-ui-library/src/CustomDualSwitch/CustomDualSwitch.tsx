// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, PropsWithChildren } from "react";
import { Grid } from "@material-ui/core";
import { Theme, makeStyles } from "@material-ui/core/styles";
import Switch, { SwitchProps } from "@material-ui/core/Switch";

/**
 * Component props
 */
interface Props extends PropsWithChildren<SwitchProps> {
  leftLabel?: string;
  rigthLabel?: string;
  isChecked?: (checked: boolean) => void;
  defaultState?: boolean;
}

/**
 * Component styles
 */
const useStyles = makeStyles((theme: Theme) => ({
  root: {
    width: 43,
    height: 19,
    padding: 1
  },
  switchBase: {
    left: 2,
    padding: 2,
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
    width: 15,
    height: 15
  },
  track: {
    borderRadius: 18,
    backgroundColor: theme.ptas.colors.theme.gray,
    opacity: 1,
    transition: theme.transitions.create(["background-color", "border"])
  },
  checked: {
    left: 7
  }
}));

/**
 * CustomSwitch
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomDualSwitch(props: Props): JSX.Element {
  const { leftLabel, rigthLabel, isChecked, defaultState } = props;
  const classes = useStyles(props);

  const [state, setState] = useState<boolean>(defaultState ?? false);

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setState(event.target.checked);
    isChecked && isChecked(event.target.checked);
  };

  return (
    <Grid component='label' container alignItems='center'>
      <Grid item>{leftLabel ?? "Yes"}</Grid>
      <Grid item>
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
          {...props}
        />
      </Grid>
      <Grid item>{rigthLabel ?? "No"}</Grid>
    </Grid>
  );
}

export default CustomDualSwitch;

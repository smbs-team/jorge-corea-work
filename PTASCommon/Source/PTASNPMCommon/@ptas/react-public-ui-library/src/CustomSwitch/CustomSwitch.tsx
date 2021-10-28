// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, ReactNode, FC } from "react";
import { createStyles, Grid, WithStyles, withStyles } from "@material-ui/core";
import { Theme } from "@material-ui/core/styles";
import FormGroup from "@material-ui/core/FormGroup";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Switch from "@material-ui/core/Switch";
import clsx from "clsx";
import { GenericWithStyles } from "@ptas/react-ui-library";
import { useUpdateEffect } from "react-use";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  label?: string | ReactNode;
  isChecked?: (checked: boolean, name?: string) => void;
  showOptions?: boolean;
  ptasVariant?: "small" | "normal" | "inverse";
  onLabel?: string | React.ReactNode;
  offLabel?: string | React.ReactNode;
  name?: string;
  value?: boolean;
}

interface classesByVariants {
  root?: string;
  switchBase?: string;
  checked?: string;
  thumb?: string;
  track?: string;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      width: 60,
      height: 30,
      padding: 1,
      margin: theme.spacing(1),
      marginTop: 0
    },
    switchBase: {
      left: 3,
      padding: 2,
      top: 1,
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
      width: 24,
      height: 24
    },
    track: {
      borderRadius: 18,
      backgroundColor: theme.ptas.colors.theme.gray,
      opacity: 1,
      transition: theme.transitions.create(["background-color", "border"])
    },
    checked: {
      left: 14
    },
    principalGrid: {
      position: "relative",
      width: "inherit"
    },
    optionOffGrid: ({ ptasVariant }: Props) => ({
      left: ptasVariant === "normal" || ptasVariant === undefined ? 18 : 9,
      bottom: -8,
      position: "absolute",
      fontSize: 12,
      fontFamily: theme.ptas.typography.bodyFontFamily
    }),
    optionOnGrid: ({ ptasVariant }: Props) => ({
      right: ptasVariant === "normal" || ptasVariant === undefined ? 15 : 8,
      bottom: -8,
      position: "absolute",
      fontSize: 12,
      fontFamily: theme.ptas.typography.bodyFontFamily
    }),
    smallRoot: {
      width: 40,
      height: 20
    },
    smallSwitchBase: {
      left: 1
    },
    smallChecked: {
      left: 4
    },
    smallThumbs: {
      width: 15,
      height: 15
    },
    inverseRoot: {
      height: 22
    },
    inverseThumb: {
      height: 17,
      width: 17,
      background: theme.ptas.colors.theme.black
    },
    inverseChecked: {
      left: 7
    },
    inverseTrack: {
      backgroundColor: theme.ptas.colors.theme.white
    },
    labelAndOptionColor: (props: Props) => ({
      color:
        props.ptasVariant === "inverse"
          ? theme.ptas.colors.theme.white
          : theme.ptas.colors.theme.black
    }),
    formControlRoot: {
      display: "flex",
      alignItems: "start"
    }
  });

/**
 * CustomSwitch
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomSwitch(props: Props): JSX.Element {
  const { label, isChecked, name, value } = props;
  const { classes } = props;

  const [state, setState] = useState<boolean>(value ?? false);

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setState(event.target.checked);
    isChecked && isChecked(event.target.checked, name);
  };

  useUpdateEffect(() => {
    setState(!!value);
  }, [value]);

  const setClassesByVariants = (): classesByVariants => {
    switch (props.ptasVariant) {
      case "small":
        return {
          root: clsx(classes.root, classes.smallRoot),
          checked: clsx(classes.checked, classes.smallChecked),
          thumb: clsx(classes.thumb, classes.smallThumbs),
          switchBase: clsx(classes.switchBase, classes.smallSwitchBase),
          track: classes.track
        };
      case "inverse":
        return {
          root: clsx(classes.root, classes.inverseRoot),
          checked: clsx(classes.checked, classes.inverseChecked),
          thumb: clsx(classes.thumb, classes.inverseThumb),
          switchBase: classes.switchBase,
          track: clsx(classes.track, classes.inverseTrack)
        };
      default:
        return {
          root: classes.root,
          checked: classes.checked,
          thumb: classes.thumb,
          switchBase: classes.switchBase,
          track: classes.track
        };
    }
  };

  return (
    <FormGroup>
      <FormControlLabel
        className={classes.formControlRoot}
        control={
          <Grid
            component='label'
            className={classes.principalGrid}
            container
            alignItems='flex-start'
            spacing={1}
          >
            {props.showOptions && (
              <Grid
                item
                className={clsx(
                  classes.optionOffGrid,
                  classes.labelAndOptionColor
                )}
              >
                {props.offLabel ?? "Off"}
              </Grid>
            )}
            <Grid item>
              <Switch
                disableRipple
                checked={state}
                onChange={handleChange}
                classes={{
                  root: setClassesByVariants().root,
                  switchBase: setClassesByVariants().switchBase,
                  thumb: setClassesByVariants().thumb,
                  track: setClassesByVariants().track,
                  checked: setClassesByVariants().checked
                }}
              />
            </Grid>
            {props.showOptions && (
              <Grid
                item
                className={clsx(
                  classes.optionOnGrid,
                  classes.labelAndOptionColor
                )}
              >
                {props.onLabel ?? "On"}
              </Grid>
            )}
          </Grid>
        }
        classes={{ label: classes.labelAndOptionColor }}
        label={label}
      />
    </FormGroup>
  );
}

export default withStyles(useStyles)(CustomSwitch) as FC<
  GenericWithStyles<Props>
>;

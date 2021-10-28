// RadioButtonGroup.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, ChangeEvent, useEffect } from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  Typography,
  Theme
} from "@material-ui/core";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Radio, { RadioProps } from "@material-ui/core/Radio";
import RadioGroup from "@material-ui/core/RadioGroup";
import FormControl from "@material-ui/core/FormControl";
import FormLabel from "@material-ui/core/FormLabel";
import clsx from "clsx";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  onChange: (value: string) => void;
  items: Item[];
  formLabel?: string;
  defaultValue?: string;
  radioProps?: RadioProps;
  value?: string;
  orientation?: "column" | "row";
  ptasVariant?: "large" | "normal" | "small";
}

interface Item {
  value: string | number;
  label: string;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {},
    label: {
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      color: theme.ptas.colors.theme.black,
      marginRight: 20
    },
    normalLabel: {
      fontSize: theme.ptas.typography.finePrint.fontSize
    },
    smallLabel: {
      fontSize: 12
    },
    formLabel: {
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      color: theme.ptas.colors.theme.black,
      marginBottom: 2,
      marginRight: 5,
      "&.Mui-focused": {
        color: theme.ptas.colors.theme.black
      }
    },
    formControlLabelRoot: {},
    radio: { padding: 1, marginRight: 5 },
    radioIcon: {
      cursor: "pointer",
      marginRight: 1,
      borderRadius: "50%",
      width: 22,
      height: 22,
      border: `2px solid ${theme.ptas.colors.theme.white}`,
      boxShadow: `0 0 0 2px black`
    },
    normalRadioIcon: {
      width: 18,
      height: 18
    },
    smallRadioIcon: {
      width: 12,
      height: 12
    },
    radioIconChecked: {
      backgroundColor: theme.ptas.colors.utility.selection,
      boxShadow: `0 0 0 2px ${theme.ptas.colors.utility.selection}`
    }
  });
/**
 * RadioButtonGroup
 *
 * @param props - Component props
 * @returns A JSX element
 */
function RadioButtonGroup(props: Props): JSX.Element {
  const {
    classes,
    onChange,
    items,
    formLabel,
    defaultValue,
    value: valueProp,
    ptasVariant
  } = props;

  const setVariant = (): { label: string; radioIcon: string } => {
    switch (ptasVariant) {
      case "normal":
        return {
          label: clsx(classes.label, classes.normalLabel),
          radioIcon: clsx(classes.radioIcon, classes.normalRadioIcon)
        };
      case "small":
        return {
          label: clsx(classes.label, classes.smallLabel),
          radioIcon: clsx(classes.radioIcon, classes.smallRadioIcon)
        };

      default:
        return {
          label: classes.label,
          radioIcon: classes.radioIcon
        };
    }
  };

  const [value, setValue] = useState<string>(valueProp ?? defaultValue ?? "");

  useEffect(() => {
    if (valueProp) setValue(valueProp);
  }, [valueProp]);

  const handleChange = (value: string) => {
    setValue(value);
    onChange(value);
  };

  return (
    <FormControl component='fieldset' classes={{ root: classes.root }}>
      {formLabel && (
        <FormLabel component='legend' className={classes.formLabel}>
          {formLabel}
        </FormLabel>
      )}
      <RadioGroup
        aria-label='gender'
        name='gender1'
        value={value}
        row={props.orientation === "row"}
        onChange={(e: ChangeEvent<HTMLInputElement>) =>
          handleChange(e.target.value)
        }
      >
        {items.map((item) => {
          return (
            <FormControlLabel
              classes={{
                root: classes.formControlLabelRoot
              }}
              key={item.value}
              value={item.value}
              control={
                <Radio
                  icon={<span className={setVariant().radioIcon} />}
                  checkedIcon={
                    <span
                      className={clsx(
                        setVariant().radioIcon,
                        classes.radioIconChecked
                      )}
                    />
                  }
                  color='default'
                  classes={{
                    root: classes.radio
                  }}
                  {...(props.radioProps || {})}
                />
              }
              label={
                <Typography
                  variant='body2'
                  color='textPrimary'
                  className={setVariant().label}
                >
                  {item.label}
                </Typography>
              }
            />
          );
        })}
      </RadioGroup>
    </FormControl>
  );
}

export default withStyles(useStyles)(RadioButtonGroup);

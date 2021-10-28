// CustomRange.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, Fragment, useState, useEffect } from "react";
import {
  Typography,
  Theme,
  WithStyles,
  createStyles,
  withStyles
} from "@material-ui/core";
import Slider from "@material-ui/core/Slider";
import { ReactComponent as SliderIcon } from "../assets/icons/slider.svg";
import { GenericWithStyles } from "@ptas/react-ui-library";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  disabled?: boolean;
  fullyRounded?: boolean;
  isActive?: boolean;
  label?: string | JSX.Element;
  max?: number;
  min?: number;
  value?: number;
  onChange?: (event: React.ChangeEvent<{}>, value: number) => void;
  defaultValue?: number;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      color: "transparent",
      height: 12,
      minWidth: 300,
      width: 100
    },
    thumb: {
      top: 18,
      textAlign: "center",
      "&:focus, &:hover, &$active": {
        boxShadow: "inherit"
      }
    },
    active: {},
    valueLabel: {
      position: "absolute",
      top: 21,
      width: 75,
      color: theme.ptas.colors.theme.black,
      fontSize: theme.ptas.typography.finePrintBold.fontSize,
      fontWeight: theme.ptas.typography.bodyBold.fontWeight
    },
    track: {
      height: 12,
      borderRadius: 12,
      background: "transparent"
    },
    rail: {
      height: 12,
      borderRadius: 12,
      background: "linear-gradient(90deg, #43A047 0.11%, #00FF22 100.11%)",
      opacity: 1
    },
    label: {}
  });

/**
 * CustomRange
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomRange(props: Props): JSX.Element {
  const { classes, label, onChange, max, min, defaultValue } = props;
  const [value, setValue] = useState<number>(0);

  useEffect(() => {
    setValue(props.value ?? 0);
  }, [props.value]);

  const renderLabel = (): JSX.Element => {
    if (min === value || value === 0) {
      return <span className={classes.valueLabel}>Not started</span>;
    }

    if (max !== value || value !== 100) {
      return <span className={classes.valueLabel}>In progress</span>;
    }

    return <span className={classes.valueLabel}>Complete</span>;
  };

  const SliderIconComponent = (props: any) => {
    return (
      <span {...props}>
        <SliderIcon />
        {renderLabel()}
      </span>
    );
  };

  const handleChange = (event: any, inputValue: number): void => {
    setValue(inputValue);

    if (onChange) onChange(event, inputValue);
  };

  return (
    <Fragment>
      {label && (
        <Typography gutterBottom className={classes.label}>
          {label}
        </Typography>
      )}
      <Slider
        valueLabelDisplay='on'
        classes={{
          root: classes.root,
          thumb: classes.thumb,
          track: classes.track,
          rail: classes.rail
        }}
        ThumbComponent={SliderIconComponent}
        onChange={handleChange}
        value={value}
        max={max ?? 100}
        min={min ?? 0}
        defaultValue={defaultValue}
      ></Slider>
    </Fragment>
  );
}

export default withStyles(useStyles)(CustomRange) as FC<
  GenericWithStyles<Props>
>;

// ColorPicker.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  Popover,
  PopoverProps,
  withStyles,
  WithStyles,
  hexToRgb,
  rgbToHex,
  Theme,
  createStyles
} from "@material-ui/core";
import React, { useState, useEffect, createRef } from "react";
import { ChromePicker, ColorResult } from "react-color";
import { PickRequiredKeys, GenericWithStyles } from "../common/types";
import CustomTextField from "../CustomTextField";

type ChangeData = ColorResult & { rgbaStr: string };

interface Props extends WithStyles<typeof useStyles> {
  /**
   * A title text that appears before the color anchor element
   */
  title?: string;
  /**
   * rgb or rgba color applied to the anchor element and to the color picker
   */
  rgbColor?: string;
  /**
   * Hexadecimal color applied to the anchor element and to the color picker, this value is converted to rgba in order to
   * use the color picker rgba slider
   */
  hexColor?: string;
  /**
   * Shows or hide the text field that prints the selected color
   */
  showHexInput?: boolean;
  /**
   * Color picker change event
   */
  onChange?: (val: ChangeData) => void;
  /**
   * Color picker change complete event
   */
  onChangeComplete?: (val: ChangeData) => void;
  /**
   * Input change event
   */
  onInputChange?: (rgbaStr: string) => void;
  /**
   * Omit optional popover props
   */
  omitPopOverProps?: [keyof Omit<PopoverProps, PickRequiredKeys<PopoverProps>>];
  /**
   * Pop over props that will be merged, required keys are excluded from this list
   */
  popOverProps?: Omit<PopoverProps, PickRequiredKeys<PopoverProps>>;
  /**
   *  onAnchorElMouseDown mouse down event
   */
  onAnchorElMouseDown?: (
    event: React.MouseEvent<HTMLDivElement, MouseEvent>
  ) => void;
  /**
   *  Label for the text field
   */
  label?: string;
  /**
   *  Disable alpha slider from color picker
   */
  disableAlpha?: boolean;
}

export type ColorPickerProps = GenericWithStyles<Props>;

const getRgbaStr = (val: ColorResult): string => {
  if (typeof val.rgb !== "object") return val.rgb;
  return `rgba(${Object.values(val.rgb).join()})`;
};

const genChangeData = (val: ColorResult): ChangeData => ({
  ...val,
  rgbaStr: getRgbaStr(val)
});

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      display: "flex",
      justifyContent: "flex-start",
      alignItems: "center",
      width: "fit-content"
    },
    box: {
      borderRadius: "50%",
      border: "1px solid black",
      width: 36,
      height: 36,
      flexShrink: 0,
      cursor: "pointer",
      "& + label": {
        marginLeft: theme.spacing(2)
      },
      "label + &": {
        marginLeft: theme.spacing(2)
      }
    },
    title: {
      textAlign: "right",
      width: "100%",
      wordWrap: "break-word",
      overflowX: "hidden"
    },
    colorHex: {
      textAlign: "left",
      width: "60px"
    },
    textField: {
      marginLeft: theme.spacing(1),
      "& .MuiInputBase-input": {
        width: "160px"
      }
    }
  });

const ColorPicker = (props: Props): JSX.Element => {
  const {
    onChange,
    onChangeComplete,
    onInputChange,
    classes,
    title,
    rgbColor,
    hexColor
  } = props;
  const [color, setColor] = useState<ColorResult>();
  const [anchorEl, setAnchorEl] = React.useState<HTMLDivElement | null>(null);
  const [inputValue, setInputValue] = React.useState<string>("");
  const anchorElRef = createRef<HTMLDivElement>();

  useEffect(() => {
    if (hexColor) {
      setColor({ hex: hexColor, rgb: hexToRgb(hexColor) } as any);
      setInputValue(hexColor);
      return;
    }
    if (rgbColor) {
      setColor({ hex: rgbToHex(rgbColor), rgb: rgbColor } as any);
      setInputValue(rgba2hexa(rgbColor));
    }
  }, [hexColor, rgbColor]);

  const isValidColor = (color: string): boolean => {
    var el = document.createElement("div");
    el.style.backgroundColor = color;
    return el.style.backgroundColor ? true : false;
  };

  const _onChange = (val: ColorResult): void => {
    setColor(val);

    if (color?.rgb.a! < 1) {
      setInputValue(rgba2hexa(getRgbaStr(val)));
    } else {
      setInputValue(val.hex);
    }

    onChange && onChange(genChangeData(val));
  };

  const _onChangeComplete = (val: ColorResult): void => {
    onChangeComplete && onChangeComplete(genChangeData(val));
  };

  const handleClick = (event: React.MouseEvent<HTMLDivElement>): void => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => setAnchorEl(null);

  const handleInputOnChange = (value: string) => {
    setInputValue(value);
    if (isValidColor(value)) {
      if (value.startsWith("#")) {
        setColor({ rgb: hexToRgb(value) } as any);
        onInputChange && onInputChange(hexToRgb(value));
      } else {
        setColor({ rgb: value } as any);
        onInputChange && onInputChange(value);
      }
    }
  };

  const _popOverProps: PopoverProps = {
    id: "" + Date.now(),
    open: !!anchorEl,
    anchorEl: anchorEl,
    onClose: handleClose,
    anchorOrigin: {
      vertical: "bottom",
      horizontal: "right"
    },
    transformOrigin: {
      vertical: "top",
      horizontal: "left"
    },
    ...(props.popOverProps || {})
  };

  if (props?.omitPopOverProps) {
    for (const k of props.omitPopOverProps) {
      delete _popOverProps[k];
    }
  }

  const rgba2hexa = (color: any) => {
    if (!isValidColor(color)) return "transparent";
    let hasChar: boolean = false;
    if (color.startsWith("#", 0)) hasChar = true;
    let a,
      rgb = color
        .replace(/\s/g, "")
        .match(/^rgba?\((\d+),(\d+),(\d+),?([^,\s)]+)?/i),
      alpha = ((rgb && rgb[4]) || "").trim(),
      hex = rgb
        ? (rgb[1] | (1 << 8)).toString(16).slice(1) +
          (rgb[2] | (1 << 8)).toString(16).slice(1) +
          (rgb[3] | (1 << 8)).toString(16).slice(1)
        : color;

    if (alpha !== "") {
      a = alpha;
    } else {
      a = "01";
    }

    a = ((a * 255) | (1 << 8)).toString(16).slice(1);
    hasChar ? (hex = hex + a) : (hex = "#" + hex + a);
    return hex;
  };

  return (
    <div className={classes.root}>
      {title && <label className={classes.title}>{title}</label>}
      <div
        ref={anchorElRef}
        onMouseDown={props.onAnchorElMouseDown}
        className={classes.box}
        style={{
          backgroundColor: inputValue ? inputValue : "transparent"
        }}
        onClick={handleClick}
      ></div>
      {props.showHexInput && (
        <CustomTextField
          label={props.label}
          value={inputValue}
          classes={{ root: classes.textField }}
          onChange={(s) => handleInputOnChange(s.target.value)}
        />
      )}
      <Popover {..._popOverProps}>
        <ChromePicker
          color={color?.rgb}
          onChange={_onChange}
          onChangeComplete={_onChangeComplete}
          disableAlpha={props.disableAlpha}
        />
      </Popover>
    </div>
  );
};

export default withStyles(useStyles)(ColorPicker);

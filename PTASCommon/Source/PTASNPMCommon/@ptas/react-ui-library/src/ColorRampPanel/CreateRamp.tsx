// CreateRamp.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useEffect, useState } from "react";
import { Box, Button, makeStyles } from "@material-ui/core";
import { RadioButtonGroup } from "../RadioButtonGroup";
import ColorPicker from "../ColorPicker";
import CustomIconButton from "../CustomIconButton";
import CustomTextField from "../CustomTextField";
import { generatePalette, getRandColorsFromHSV } from "../common/colorUtil";
import clsx from "clsx";
import RefreshIcon from "@material-ui/icons/Refresh";
import ColorRamp from "./ColorRamp";
import { ColorConfiguration } from "./types";
import { v4 as uuidv4 } from "uuid";

interface Props {
  onCancel?: () => void;
  onSave?: (colorConfig: ColorConfiguration) => void;
  colorConfig?: ColorConfiguration;
  maxNumColors?: number;
}

const useStyles = makeStyles((theme) => ({
  root: {
    width: 585,
    padding: theme.spacing(1.75, 3, 3, 3),
    position: "relative",
    boxSizing: "unset"
  },
  value: {
    width: 80
  },
  details: {
    display: "flex",
    marginBottom: theme.spacing(2)
  },
  inputColors: {
    display: "flex",
    marginBottom: theme.spacing(2)
  },
  valueContainer: {
    flexGrow: 1,
    flexDirection: "column",
    display: "flex",
    height: "fit-content",
    width: 200
  },
  message: {
    marginTop: theme.spacing(2)
  },
  radioButtonGroup: {
    flexGrow: 1
  },
  valueLabel: {
    paddingRight: theme.spacing(1.75)
  },
  labels: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize
  },
  titleLabel: {
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontWeight: "bold",
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    paddingBottom: theme.spacing(3),
    display: "block"
  },
  inputColorsLabel: {
    fontWeight: "bold",
    display: "block",
    marginBottom: theme.spacing(2)
  },
  colorPicker: {
    marginBottom: theme.spacing(2.5)
  },
  sequencialContainer: {
    flexGrow: 1
  },
  colorRamp: {
    width: "unset !important",
    justifyContent: "center",
    display: "unset !important"
  },
  buttonGroup: {
    textAlign: "end"
  },
  button: {
    width: 129,
    height: 32,
    backgroundColor: "#169db3",
    stroke: "#0a5f78",
    strokeWidth: "1px",
    color: theme.ptas.colors.theme.white,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.body.fontSize,
    "&:hover": {
      backgroundColor: "#169db3",
      boxShadow: "0px 7px 21px 0px rgba(128,161,184,0.69)"
    }
  },
  radioRow: {
    marginBottom: theme.spacing(2)
  },
  radioChecked: {
    "&::before": {
      width: 24,
      height: 24
    }
  },
  rampRowContainer: {
    marginBottom: theme.spacing(2.5) + "px !important",
    height: 40
  },
  colorBox: {
    width: "unset !important",
    height: "100% !important",
    display: "block",
    flexGrow: 1
  },
  randomIcon: {
    color: "black",
    marginBottom: theme.spacing(1.5)
  },
  numColors: {
    display: "flex",
    alignItems: "center"
  }
}));

/**
 * CreateRamp
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CreateRamp(props: Props): JSX.Element {
  const classes = useStyles();

  const [numColors, setNumColors] = useState<number>(props.maxNumColors ?? 22);
  const [radioSelection, setRadioSelection] = useState<string>();
  const [colorConfig, setColorConfig] = useState<ColorConfiguration>();
  const [colors, setColors] = useState<string[]>([]);

  const [color1, setColor1] = useState<string>("#00429d");
  const [color2, setColor2] = useState<string>("#96ffea");
  const [color3, setColor3] = useState<string>("lightyellow");
  const [color4, setColor4] = useState<string>("#ff005e");

  const [colorSet1, setColorSet1] = useState<string[]>([]);
  const [colorSet2, setColorSet2] = useState<string[]>([]);

  useEffect(() => {
    setRadioSelection(
      (props.colorConfig && props.colorConfig.type) ?? "sequential"
    );
    setColorSet1(
      props.colorConfig?.colorSet1
        ? props.colorConfig.colorSet1
        : ["#00429d", "#96ffea"]
    );
    setColorSet2(
      props.colorConfig?.colorSet2
        ? props.colorConfig.colorSet2
        : ["lightyellow", "#ff005e"]
    );
    setColor1(
      props.colorConfig?.colorSet1 ? props.colorConfig.colorSet1[0] : "#00429d"
    );
    setColor2(
      props.colorConfig?.colorSet1 ? props.colorConfig.colorSet1[1] : "#96ffea"
    );
    setColor3(
      props.colorConfig?.colorSet2 ? props.colorConfig.colorSet2[0] : "#FFFFE0"
    );
    setColor4(
      props.colorConfig?.colorSet2 ? props.colorConfig.colorSet2[1] : "#ff005e"
    );
  }, []);

  useEffect(() => {
    setRampColors();
  }, [radioSelection, numColors, color1, color2, color3, color4]);

  const setRampColors = (): void => {
    if (!radioSelection) return;
    setColorSet1([color1, color2]);
    setColorSet2([color3, color4]);
    if (radioSelection === "random") {
      setNumColors(props.maxNumColors ?? 22);
      const colors = getRandColorsFromHSV(numColors);
      setColorConfig({
        id: props.colorConfig?.id ?? uuidv4(),
        colors: props.colorConfig?.colors ?? colors,
        type: radioSelection
      });
      setColors(props.colorConfig?.colors ?? colors);
    } else {
      const colorsToAdd = generatePalette(
        colorSet1,
        colorSet2,
        numColors,
        radioSelection === "diverging"
      );
      if (radioSelection === "sequential") {
        setColorConfig({
          id: props.colorConfig?.id ?? uuidv4(),
          colorSet1: colorSet1,
          type: radioSelection,
          colors: colorsToAdd
        });
      } else {
        setColorConfig({
          id: props.colorConfig?.id ?? uuidv4(),
          colorSet1: colorSet1,
          colorSet2: colorSet2,
          type: radioSelection,
          colors: colorsToAdd
        });
      }
      setColors(colorsToAdd);
    }
  };

  const onSave = (): void => {
    if (!colorConfig) return;
    if (radioSelection === "random") {
      props.onSave && props.onSave(colorConfig);
      return;
    }

    props.onSave &&
      props.onSave({
        ...(colorConfig as ColorConfiguration),
        colors: generatePalette(
          colorSet1,
          colorSet2,
          props.maxNumColors ?? 22,
          radioSelection === "diverging"
        )
      });
  };

  const randomize = (): void => {
    const toAdd = getRandColorsFromHSV(numColors);
    setColors(toAdd);
    setColorConfig({
      id: props.colorConfig?.id ?? uuidv4(),
      colors: toAdd,
      type: radioSelection
    });
  };

  return (
    <div className={classes.root}>
      <label className={clsx(classes.titleLabel)}>Color Ramp Detail</label>
      <Box className={classes.details}>
        <RadioButtonGroup
          classes={{
            root: classes.radioButtonGroup,
            formControlLabelRoot: classes.radioRow,
            radioIconChecked: classes.radioChecked
          }}
          defaultValue={props.colorConfig?.type ?? "sequential"}
          onChange={(selection): void => setRadioSelection(selection)}
          items={[
            { value: "sequential", label: "Sequential" },
            { value: "diverging", label: "Diverging" },
            { value: "random", label: "Random" }
          ]}
        />
        <Box className={classes.valueContainer}>
          <Box className={classes.numColors}>
            <label className={clsx(classes.valueLabel, classes.labels)}>
              Number of colors
            </label>
            <CustomTextField
              type='number'
              label='Value'
              disabled={radioSelection === "random"}
              value={numColors}
              className={classes.value}
              onChange={(event) => {
                const num = parseInt(event.target.value);
                if (!isNaN(num) && num <= 100) {
                  setNumColors(parseInt(event.target.value));
                }
              }}
              InputProps={{ inputProps: { min: 0, max: 100 } }}
            />
          </Box>
          {/* {radioSelection === "random" && (
            <Box className={clsx(classes.message, classes.labels)}>
              <label>{`
                For more than ${props.maxNumColors} colors, new random colors will be added to the
                ramp`}
              </label>
            </Box>
          )} */}
        </Box>
      </Box>
      {radioSelection === "random" && (
        <CustomIconButton
          icon={<RefreshIcon />}
          className={classes.randomIcon}
          onClick={randomize}
          text='Randomize'
        />
      )}
      {radioSelection !== "random" && (
        <Fragment>
          <label className={clsx(classes.labels, classes.inputColorsLabel)}>
            Input Colors
          </label>
          <Box className={classes.inputColors}>
            <Box className={classes.sequencialContainer}>
              <ColorPicker
                showHexInput
                classes={{ root: classes.colorPicker }}
                onChangeComplete={(e) => setColor1(e.hex)}
                hexColor={color1}
                disableAlpha
              />
              <ColorPicker
                showHexInput
                classes={{ root: classes.colorPicker }}
                onChangeComplete={(e) => setColor2(e.hex)}
                hexColor={color2}
                disableAlpha
              />
            </Box>
            {radioSelection === "diverging" && (
              <Box className={classes.sequencialContainer}>
                <ColorPicker
                  showHexInput
                  classes={{ root: classes.colorPicker }}
                  onChangeComplete={(e) => setColor3(e.hex)}
                  hexColor={color3}
                  disableAlpha
                />
                <ColorPicker
                  showHexInput
                  classes={{ root: classes.colorPicker }}
                  onChangeComplete={(e) => setColor4(e.hex)}
                  hexColor={color4}
                  disableAlpha
                />
              </Box>
            )}
          </Box>
        </Fragment>
      )}
      <ColorRamp
        colors={colors}
        hideOptions
        hideSelector
        classes={{
          root: classes.colorRamp,
          rowContainer: classes.rampRowContainer,
          colorBox: classes.colorBox
        }}
      />
      <Box className={classes.buttonGroup}>
        <Button
          className={classes.button}
          style={{ marginRight: 34 }}
          onClick={props.onCancel}
        >
          Cancel
        </Button>
        <Button className={classes.button} onClick={onSave}>
          Save
        </Button>
      </Box>
    </div>
  );
}

export default CreateRamp;

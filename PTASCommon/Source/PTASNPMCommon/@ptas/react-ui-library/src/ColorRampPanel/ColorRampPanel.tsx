// ColorRampPanel.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, useRef, useEffect } from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  Box,
  Theme,
  Slider,
  Tooltip,
  ValueLabelProps
} from "@material-ui/core";
import Scrollbar from "react-scrollbars-custom";
import AddCircleOutlineIcon from "@material-ui/icons/AddCircleOutline";
import {
  addAlphaToRgb,
  convertHexToRGBA,
  generatePalette,
  getRandColorsFromHSV
} from "../common/colorUtil";
import { GenericWithStyles } from "../common";
import CustomIconButton from "../CustomIconButton";
import CustomPopover from "../CustomPopover";
import CreateRamp from "./CreateRamp";
import ColorRamp from "./ColorRamp";
import { ColorConfiguration } from "./types";
import colors from "./defaultColors.json";
import Switch from "../Switch";
import { uniqueId } from "lodash";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  colorsToGenerate: number;
  ramps?: ColorConfiguration[];
  selectedFillRampId?: string;
  selectedOutlineRampId?: string;
  opacity?: number;

  //Should not be set, default is 16 as per requirement.
  maxNumberOfRamps?: number;

  title?: string;
  switchContent: [string, string];
  hideHeader?: boolean;
  hideOptions?: boolean;

  switchOnChange?: (index: number) => void;

  onSelect?: (selectedRamp: ColorConfiguration) => void;
  onEdit?: (newConfig: ColorConfiguration) => void;
  onDelete?: (config: ColorConfiguration) => void;
  onCreate?: (newRamp: ColorConfiguration) => void;
}

export type ColorRampPanelProps = GenericWithStyles<Props>;

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      width: "fit-content"
    },
    rowContainer: {},
    label: {
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      lineHeight: 0.5,
      fontWeight: "bold",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      cursor: "pointer",
      color: theme.ptas.colors.theme.accent,
      display: "flex",
      alignItems: "center",
      width: "fit-content",
      paddingTop: theme.spacing(1),
      paddingLeft: theme.spacing(4.5),
      "&:hover": {
        textDecoration: "underline"
      }
    },
    header: {
      display: "flex",
      alignItems: "center",
      justifyContent: "space-between",
      marginBottom: theme.spacing(1)
    },
    title: {
      fontSize: "1rem",
      fontWeight: "bold",
      fontFamily: theme.ptas.typography.titleFontFamily,
      marginBottom: theme.spacing(1)
    },
    rampContainer: {
      paddingRight: theme.spacing(1),
      margin: 0,
      width: 600
    },
    trackY: {
      background: "unset !important"
    },
    scroll: {
      maxWidth: "707px",
      minWidth: "360px",
      maxHeight: "274px"
    },
    rampColorBox: {
      width: "unset !important",
      height: "100% !important",
      display: "block",
      flexGrow: 1
    },
    sliderContainer: {
      display: "flex",
      alignItems: "center",
      fontSize: "1rem",
      fontFamily: theme.ptas.typography.titleFontFamily,
      fontWeight: "bold",
      marginBottom: 18,
      marginTop: 8
    },
    slider: {
      width: 180,
      marginLeft: 85
    },
    rampRowContainer: {
      height: 40,
      width: "100%"
    }
  });

export const getColors = (
  config: ColorConfiguration,
  numColors: number,
  opacity?: number
): string[] | undefined => {
  switch (config.type) {
    case "sequential":
      if (!config.colorSet1) return;
      const sequenConlors = generatePalette(config.colorSet1, [], numColors);
      return convertHexToRGBA(sequenConlors, opacity ?? 1);
    case "diverging":
      if (!config.colorSet1 || !config.colorSet2) return;
      const diverColors = generatePalette(
        config.colorSet1,
        config.colorSet2,
        numColors,
        true
      );
      return convertHexToRGBA(diverColors, opacity ?? 1);
    case "random":
      if (!config.colors) return;
      let toSend: string[] = [];
      if (config.colors[0].includes("rgba")) {
        toSend = config.colors;
      } else {
        toSend = config.colors
          ? addAlphaToRgb([...config.colors], opacity ?? 1)
          : [];
      }

      const totalColors = numColors;
      if (totalColors > 22)
        return toSend?.concat(
          addAlphaToRgb(getRandColorsFromHSV(totalColors - 22), opacity ?? 1)
        );
      if (totalColors < 22) toSend?.splice(totalColors, toSend.length);
      return toSend;
  }
  return [];
};

/**
 * ColorRampPanel
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ColorRampPanel(props: Props): JSX.Element {
  const { classes } = props;

  const rampsEndRef = useRef<HTMLDivElement>(null);

  const [ramps, setRamps] = useState<ColorConfiguration[]>([]);
  const [event, setEvent] = useState<HTMLElement | null>(null);
  const [isDelete, setIsDelete] = useState<boolean>(false);

  const [selectedTab, setSelectedTab] = useState<number>(0);
  const [selectedIndex, setSelectedIndex] = useState<number>();

  const [selectedIndex1, setSelectedIndex1] = useState<number>(-1);
  const [selectedIndex2, setSelectedIndex2] = useState<number>(-1);
  const [selectedRamp, setSelectedRamp] = useState<ColorConfiguration>();

  const getDefaultOpacity = (): number => {
    if (!props.opacity || props.opacity > 1 || props.opacity < 0) return 1;
    return props.opacity;
  };

  const [opacity, setOpacity] = useState<number>(getDefaultOpacity());

  useEffect(() => {
    if (isDelete) {
      setIsDelete(false);
      return;
    }
    scrollToBottom();
  }, [ramps]);

  useEffect(() => {
    setSelectedIndex1(
      ramps.findIndex((r) => r.id === props.selectedFillRampId)
    );
  }, [props.selectedFillRampId]);

  useEffect(() => {
    setSelectedIndex2(
      ramps.findIndex((r) => r.id === props.selectedOutlineRampId)
    );
  }, [props.selectedOutlineRampId]);

  useEffect(() => {
    if (props.selectedFillRampId && selectedTab === 0)
      setSelectedRamp(ramps.find((r) => r.id === props.selectedFillRampId));
    if (props.selectedOutlineRampId && selectedTab === 1)
      setSelectedRamp(ramps.find((r) => r.id === props.selectedOutlineRampId));
  }, [ramps]);

  useEffect(() => {
    if (ramps.length < 1) return;

    if (selectedIndex1 === -1 && selectedIndex2 === -1) {
      setSelectedIndex1(
        ramps.findIndex((r) => r.id === props.selectedFillRampId)
      );
      setSelectedIndex2(
        ramps.findIndex((r) => r.id === props.selectedOutlineRampId)
      );
    }
  }, [ramps]);

  useEffect(() => {
    selectedTab === 0
      ? setSelectedIndex(selectedIndex1)
      : setSelectedIndex(selectedIndex2);
  }, [selectedTab, selectedIndex1, selectedIndex2]);

  const getRandomColors = (ramp: ColorConfiguration): string[] => {
    if (!ramp.colors) return [];
    const totalColors = ramp.colors.length;
    if (totalColors === props.colorsToGenerate) return ramp.colors;
    if (totalColors < props.colorsToGenerate) {
      return [
        ...ramp.colors,
        ...getRandColorsFromHSV(props.colorsToGenerate - totalColors)
      ];
    }
    return ramp.colors.splice(0, props.colorsToGenerate);
  };

  useEffect(() => {
    const toAdd: ColorConfiguration[] = [
      {
        id: "defaultRamp1",
        colors: colors.defaultRamp1,
        type: "random"
      },
      {
        id: "defaultRamp2",
        colors: colors.defaultRamp2,
        type: "random"
      },
      {
        id: "defaultRamp3",
        colors: colors.defaultRamp3,
        type: "random"
      }
    ];
    if (props.ramps && props.ramps.length >= 1) {
      props.ramps.forEach((r) => {
        toAdd.push(
          r.type !== "random"
            ? { ...r, colors: getColors(r, props.colorsToGenerate, opacity) }
            : { ...r, colors: getRandomColors(r) }
        );
      });
    }
    setRamps(toAdd);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.ramps]);

  const scrollToBottom = () => {
    rampsEndRef.current?.scrollIntoView({ behavior: "smooth" });
  };

  const handleOnSelect = (config: ColorConfiguration, rampIndex: number) => {
    setSelectedRamp(config);
    selectedTab === 0
      ? setSelectedIndex1(rampIndex)
      : setSelectedIndex2(rampIndex);
    props.onSelect &&
      props.onSelect({
        ...config,
        colors: getColors(config, props.colorsToGenerate, opacity) ?? [],
        selectedTab: selectedTab,
        opacity: opacity
      });
  };

  const handleRampOnSave = (list: ColorConfiguration): void => {
    if (!list) return;
    setRamps([...ramps, { ...list }]);
    props.onCreate &&
      props.onCreate({
        ...list,
        colors: getColors(list, props.colorsToGenerate, opacity),
        opacity: opacity
      });
    setEvent(null);
  };

  const handleDelete = (
    colorConfig: ColorConfiguration,
    index: number
  ): void => {
    setIsDelete(true);
    setRamps(ramps.filter((_r, i) => i !== index));
    props.onDelete && props.onDelete(colorConfig);
  };

  const handleEdit = (config: ColorConfiguration): void => {
    props.onEdit &&
      props.onEdit({
        ...config,
        colors: getColors(config, props.colorsToGenerate, opacity),
        opacity: opacity
      });
  };

  const ValueLabelComponent = (props: ValueLabelProps) => {
    const { children, open, value } = props;

    return (
      <Tooltip open={open} enterTouchDelay={0} placement='top' title={value}>
        {children}
      </Tooltip>
    );
  };

  return (
    <Box className={classes.root}>
      {!props.hideHeader && (
        <Box className={classes.header}>
          <label className={classes.title}>{props.title}</label>
          <Switch
            items={props.switchContent}
            onSelected={(s) => {
              props.switchOnChange && props.switchOnChange(s);
              setSelectedTab(s);
            }}
          />
        </Box>
      )}
      <Scrollbar
        noScrollX
        translateContentSizesToHolder
        disableTracksWidthCompensation
        className={classes.scroll}
        trackYProps={{
          renderer: (props) => {
            const { elementRef, ...restProps } = props;
            return (
              <span
                {...restProps}
                ref={elementRef}
                className={classes.trackY}
              />
            );
          }
        }}
      >
        {ramps.map((item, i) => {
          return (
            <ColorRamp
              key={uniqueId("ramp_")}
              colors={item.colors ?? []}
              opacity={opacity}
              colorConfig={item}
              onSelect={(config) => {
                handleOnSelect(config, i);
              }}
              onDelete={(config) => handleDelete(config, i)}
              onEdit={handleEdit}
              isSelected={i === selectedIndex}
              classes={{
                root: classes.rampContainer,
                colorBox: classes.rampColorBox,
                rowContainer: classes.rampRowContainer
              }}
              hideOptions={props.hideOptions}
              numberOfColorsPerRamp={props.colorsToGenerate}
            />
          );
        })}
        <div ref={rampsEndRef} />
      </Scrollbar>
      <Box>
        <Box className={classes.sliderContainer}>
          <label>Transparency</label>
          <Slider
            ValueLabelComponent={ValueLabelComponent}
            step={0.01}
            min={0}
            max={1}
            defaultValue={getDefaultOpacity()}
            className={classes.slider}
            onChange={(_c, value) => {
              setOpacity(value as number);
            }}
            onChangeCommitted={(): void => {
              if (!selectedRamp) return;
              props.onSelect &&
                props.onSelect({
                  ...selectedRamp,
                  colors:
                    getColors(selectedRamp, props.colorsToGenerate, opacity) ??
                    [],
                  selectedTab: selectedTab,
                  opacity: opacity
                });
            }}
          />
        </Box>
        <CustomIconButton
          icon={<AddCircleOutlineIcon />}
          text='New color ramp'
          onClick={(e) => setEvent(e.currentTarget)}
          disabled={ramps.length === props.maxNumberOfRamps ?? 16}
        />
      </Box>
      <CustomPopover
        showCloseButton
        anchorEl={event}
        onClose={(): void => {
          setEvent(null);
        }}
      >
        <CreateRamp
          onCancel={() => setEvent(null)}
          onSave={handleRampOnSave}
          maxNumColors={props.colorsToGenerate}
        />
      </CustomPopover>
    </Box>
  );
}

export default withStyles(useStyles)(ColorRampPanel);

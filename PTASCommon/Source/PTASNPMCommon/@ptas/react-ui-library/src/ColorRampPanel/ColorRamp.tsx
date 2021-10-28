// ColorRamp.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, Fragment, useEffect, useState } from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  Theme,
  Box
} from "@material-ui/core";
import { GenericWithStyles } from "../common/types";
import clsx from "clsx";
import CustomPopover from "../CustomPopover";
import CustomIconButton from "../CustomIconButton";
import CreateIcon from "@material-ui/icons/Create";
import ClearIcon from "@material-ui/icons/Clear";
import CreateRamp from "./CreateRamp";
import { ColorConfiguration } from "./types";
import { uniqueId } from "lodash";
import Alert from "../PopoverContent/Alert/Alert";

/**
 * Component props
 */
export interface ColorRampProps extends WithStyles<typeof useStyles> {
  colors: string[];
  colorConfig?: ColorConfiguration;
  isSelected?: boolean;
  numberOfColorsPerRamp?: number;
  hideSelector?: boolean;
  hideOptions?: boolean;
  opacity?: number;
  onEdit?: (newConfig: ColorConfiguration) => void;
  onDelete?: (config: ColorConfiguration) => void;
  onSelect?: (config: ColorConfiguration) => void;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      display: "flex",
      alignItems: "center",
      width: "fit-content",
      marginBottom: theme.spacing(1 / 4),
      marginTop: theme.spacing(1 / 4)
    },
    colorBox: {
      width: 40,
      height: 40
      //boxShadow: "0px 0px 2px gray",
    },
    rowContainer: {
      display: "flex",
      marginBottom: theme.spacing(1),
      background: "#eee",
      padding: theme.spacing(1 / 2)
    },
    radioIcon: {
      position: "relative",
      top: -3,
      cursor: "pointer",
      marginRight: theme.spacing(1.75),
      marginLeft: theme.spacing(0.25),
      borderRadius: "50%",
      width: 25,
      height: 20,
      border: `2px solid ${theme.ptas.colors.theme.white}`,
      boxShadow: `0 0 0 2px black`
    },
    radioIconChecked: {
      backgroundColor: theme.ptas.colors.utility.selection,
      boxShadow: `0 0 0 2px ${theme.ptas.colors.utility.selection}`
    },
    optionsIconButton: {
      top: -3
    },
    optionsIcons: {
      marginLeft: theme.spacing(1),
      color: theme.ptas.colors.theme.black,
      "&:hover": {
        color: theme.ptas.colors.utility.selection
      }
    },
    alertCloseButton: {
      color: theme.ptas.colors.theme.white
    }
  });

/**
 * ColorRamp
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ColorRamp(props: ColorRampProps): JSX.Element {
  const { classes, onSelect, hideSelector } = props;
  const [event, setEvent] = useState<HTMLElement | null>(null);
  const [deleteEvent, setDeleteEvent] = useState<HTMLElement | null>(null);
  const [colors, setColors] = useState<string[]>(props.colors);
  const [colorConfig, setColorConfig] = useState<ColorConfiguration>(
    props.colorConfig ?? { id: uniqueId("ztemp_") }
  );

  useEffect(() => {
    setColors(props.colors);
  }, [props.colors]);

  const handleLabelClick = () => {
    onSelect && onSelect(colorConfig);
  };

  const handleRampOnSave = (list: ColorConfiguration): void => {
    if (!list) return;
    setColors(list.colors as string[]);
    props.onEdit && props.onEdit(list);
    setColorConfig(list);
    setEvent(null);
  };

  const defaultRamps: string[] = [
    "defaultRamp1",
    "defaultRamp2",
    "defaultRamp3"
  ];
  const isDisabled: boolean = defaultRamps.includes(colorConfig.id);

  return (
    <Box className={classes.root}>
      {!hideSelector &&
        (props.isSelected ? (
          <span className={clsx(classes.radioIcon, classes.radioIconChecked)} />
        ) : (
          <span className={classes.radioIcon} onClick={handleLabelClick} />
        ))}
      <Box className={classes.rowContainer}>
        {colors.map((color) => (
          <div
            key={color}
            className={classes.colorBox}
            style={{
              backgroundColor: color,
              opacity: props.opacity
            }}
          />
        ))}
      </Box>
      {!props.hideOptions && (
        <Fragment>
          <CustomIconButton
            onClick={(e) => setEvent(e.currentTarget)}
            icon={
              <CreateIcon
                className={classes.optionsIcons}
                style={{ color: isDisabled ? "gray" : "" }}
              />
            }
            disableRipple={false}
            className={classes.optionsIconButton}
            disabled={isDisabled}
          />
          <CustomIconButton
            icon={
              <ClearIcon
                className={classes.optionsIcons}
                style={{ color: isDisabled ? "gray" : "" }}
              />
            }
            disableRipple={false}
            onClick={(e) => setDeleteEvent(e.currentTarget)}
            className={classes.optionsIconButton}
            disabled={isDisabled}
          />
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
              colorConfig={colorConfig}
              maxNumColors={props.numberOfColorsPerRamp}
            />
          </CustomPopover>
          <CustomPopover
            showCloseButton
            anchorEl={deleteEvent}
            onClose={(): void => {
              setDeleteEvent(null);
            }}
            classes={{ closeButton: classes.alertCloseButton }}
          >
            <Alert
              contentText='Delete will permanently erase this ramp'
              okButtonText='Delete this ramp'
              okButtonClick={() => {
                props.onDelete && props.onDelete(colorConfig);
                setDeleteEvent(null);
              }}
            />
          </CustomPopover>
        </Fragment>
      )}
    </Box>
  );
}

export default withStyles(useStyles)(ColorRamp) as FC<
  GenericWithStyles<ColorRampProps>
>;

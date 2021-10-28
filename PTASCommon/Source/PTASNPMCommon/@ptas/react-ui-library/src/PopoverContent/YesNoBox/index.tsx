// YesNoBox.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useCallback, PropsWithChildren } from "react";
import {
  withStyles,
  WithStyles,
  Box,
  Typography,
  Theme,
  createStyles
} from "@material-ui/core";
import { CustomButton } from "../../CustomButton";

const styles = (theme: Theme) =>
  createStyles({
    root: {
      flexGrow: 1,
      maxWidth: 300,
      margin: theme.spacing(2, 6, 4, 4)
    },
    header: {},
    body: {
      display: "flex",
      flexDirection: "column",
      justifyContent: "center"
    },
    bodyRow: {
      display: "flex",
      alignItems: "center",
      marginTop: theme.spacing(4)
    },
    textFieldContainer: {
      marginRight: theme.spacing(22 / 8),
      width: 230
    },
    select: {
      borderRadius: 0,
      borderColor: "#c4c4c4",
      "&:hover": {
        borderColor: "black"
      }
    },
    selectContainer: {
      marginRight: theme.spacing(22 / 8),
      width: 230
    },
    checkBoxContainer: {},
    switchChecked: {
      color: theme.ptas.colors.utility.selection
    },
    switchTrack: {
      backgroundColor: theme.ptas.colors.theme.black
    },
    selectIcon: {
      borderRadius: 0,
      border: "none",
      background: "none"
    },
    buttonYes: {
      marginRight: 0
    },
    buttonNo: {
      marginLeft: "auto",
      marginRight: theme.spacing(2)
    },
    title: {},
    customIconButton: {},
    closeButton: {
      position: "absolute",
      top: theme.spacing(2),
      right: theme.spacing(2),
      cursor: "pointer"
    },
    closeIcon: {}
  });

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles> {
  title: string;
  buttonTextYes?: string;
  buttonTextNo?: string;
  clickYes: () => void;
  clickNo: () => void;
  cancelClick: () => void;
}

/**
 * YesNoBox
 *
 * @param props - Component props
 * @returns A JSX element
 */
function YesNoBox(props: PropsWithChildren<Props>): JSX.Element {
  const {
    classes,
    title,
    clickYes,
    clickNo,
    buttonTextYes,
    buttonTextNo,
    children
  } = props;

  const _onAcceptButtonClick = useCallback(
    (_e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
      clickYes && clickYes();
    },
    []
  );

  const _onRejectButtonClick = useCallback(
    (_e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
      clickNo && clickNo();
    },
    []
  );

  return (
    <div className={classes.root}>
      <Box className={classes.header}>
        <Typography variant={"body1"} className={classes.title}>
          {title}
        </Typography>
      </Box>
      <Box className={classes.body}>
        <Box className={classes.bodyRow}></Box>
      </Box>
      {children && <Box className={classes.bodyRow}>{children}</Box>}
      <Box className={classes.bodyRow}>
        <CustomButton
          onClick={_onRejectButtonClick}
          classes={{ root: classes.buttonNo }}
          fullyRounded
        >
          {buttonTextNo ?? "No"}
        </CustomButton>
        <CustomButton
          onClick={_onAcceptButtonClick}
          classes={{ root: classes.buttonYes }}
          fullyRounded
        >
          {buttonTextYes ?? "Yes"}
        </CustomButton>
      </Box>
    </div>
  );
}

export default withStyles(styles)(YesNoBox);

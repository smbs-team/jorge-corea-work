// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useState } from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  Box,
  Theme
} from "@material-ui/core";
import Tooltip from "@material-ui/core/Tooltip";
import OptionsMenu, { MenuOption } from "../OptionsMenu/";
import ErrorIcon from "@material-ui/icons/Error";
import HelpOutlineIcon from "@material-ui/icons/HelpOutline";
import CustomIconButton from "../CustomIconButton";
import LockIcon from "@material-ui/icons/Lock";
import LockOpenIcon from "@material-ui/icons/LockOpen";
import CheckCircleIcon from "@material-ui/icons/CheckCircle";
import Loader from "react-loader-spinner";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  imgSrc?: string;
  title?: string;
  author?: string;
  date?: string;
  isLocked?: boolean;
  onClick?: () => void;
  error?: string;
  showLock?: boolean;
  onMenuOptionClick?: (option: MenuOption) => void;
  onLockClick?: (isLocked: boolean) => void;
  disableOptions?: boolean;
  menuItems?: MenuOption[];
  status?: string;
  header?: string;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      display: "flex",
      alignItems: "center",
      flexDirection: "column",
      position: "relative",
      width: 200,
      height: 220,
      border: "1px solid",
      borderColor: theme.ptas.colors.theme.grayLight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      cursor: "pointer"
    },
    content: {
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
      backgroundColor: "#f7f6f6",
      width: "100%",
      height: "100%"
    },
    footer: {
      width: "100%",
      height: 53,
      borderTop: "1px solid",
      borderColor: theme.ptas.colors.theme.grayLight,
      marginTop: "auto",
      flexShrink: 0,
      position: "relative"
    },
    title: {
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      textAlign: "center",
      display: "block",
      height: 22,
      width: "100%",
      overflow: "hidden",
      whiteSpace: "nowrap",
      textOverflow: "ellipsis"
    },
    details: {
      marginLeft: theme.spacing(1),
      fontSize: "0.625rem"
    },
    defaultIcon: {
      fontSize: 80,
      color: "#aaaaaa"
    },
    image: {
      width: "100%",
      height: "165px"
    },
    author: {
      display: "block",
      color: theme.ptas.colors.theme.grayDark
    },
    date: {
      display: "block",
      color: theme.ptas.colors.theme.gray
    },
    options: {
      position: "absolute",
      boxShadow: `0px 2px 6px 1px rgba(0,0,0,0.2),0 6px 20px 0 rgba(0,0,0,0.19)`,
      right: 0,
      backgroundColor: theme.ptas.colors.theme.white,
      width: 28,
      height: 20,
      textAlign: "center"
    },
    optionsIcon: {
      color: theme.ptas.colors.theme.black
    },
    iconButton: {
      fontSize: "1.4rem",
      position: "absolute",
      bottom: 2,
      right: 7,
      margin: 0,
      verticalAlign: "top",
      "&:hover": {
        backgroundColor: "unset"
      }
    },
    errorIcon: {
      position: "absolute",
      color: theme.ptas.colors.utility.error,
      bottom: 2,
      right: 6
    },
    progressIcon: {
      position: "absolute",
      color: "blue",
      bottom: 2,
      right: 6
    },
    successIcon: {
      position: "absolute",
      color: theme.ptas.colors.utility.success,
      bottom: 2,
      right: 6
    },
    lockIcon: {
      color: "#aaaaaa",
      fontSize: 21
    },
    lockIconButton: {},
    titleContainer: {
      display: "flex"
    }
  });

/**
 * ChartCard
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ChartCard(props: Props): JSX.Element {
  const { classes } = props;
  const [isShown, setIsShown] = useState(false);
  const [isLocked, setIsLocked] = useState<boolean>(props.isLocked ?? false);

  const getStatus = (): JSX.Element => {
    switch (props.status?.toLowerCase()) {
      case "inprogress":
        return (
          <Tooltip title={"In Progress"} placement='bottom'>
            <div className={classes.progressIcon}>
              <Loader type='Oval' color='#00BFFF' height={15} width={15} />
            </div>
          </Tooltip>
        );
      case "failed":
        return (
          <Tooltip title={"Error"} placement='bottom'>
            <ErrorIcon className={classes.errorIcon} />
          </Tooltip>
        );
      case "finished":
        return (
          <Tooltip title={"Success"} placement='bottom'>
            <CheckCircleIcon className={classes.successIcon} />
          </Tooltip>
        );
      default:
        return <Fragment></Fragment>;
    }
  };

  return (
    <Box
      className={classes.root}
      onMouseEnter={() => setIsShown(true)}
      onMouseLeave={() => setIsShown(false)}
    >
      {props.header && (
        <Box
          className={classes.title}
          style={{ color: "green", marginTop: "4px" }}
        >
          {props.header ?? "Sample Header"}
        </Box>
      )}
      {!props.disableOptions && isShown && (
        <span className={classes.options}>
          <OptionsMenu
            onItemClick={(selection) => {
              if (selection.id === "lockUnlock") setIsLocked(!isLocked);
              props.onMenuOptionClick && props.onMenuOptionClick(selection);
            }}
            classes={{
              icon: classes.optionsIcon,
              iconButtonDots: classes.iconButton
            }}
            items={[
              ...(props.menuItems as MenuOption[]),
              props.showLock
                ? { id: "lockUnlock", label: isLocked ? "Unlock" : "Lock" }
                : { id: "remove", label: "remove" }
            ].filter((m) => m.id !== "remove")}
            showTail={false}
          />
        </span>
      )}
      <Box className={classes.content} onClick={props.onClick}>
        {props.imgSrc ? (
          <img src={props.imgSrc} className={classes.image} />
        ) : (
          <HelpOutlineIcon className={classes.defaultIcon} />
        )}
      </Box>
      <Box className={classes.footer}>
        <Box className={classes.titleContainer}>
          {props.showLock && (
            <CustomIconButton
              className={classes.lockIconButton}
              icon={
                isLocked ? (
                  <LockIcon className={classes.lockIcon} />
                ) : (
                  <LockOpenIcon className={classes.lockIcon} />
                )
              }
              onClick={(): void => {
                setIsLocked(!isLocked);
                props.onLockClick && props.onLockClick(!isLocked);
              }}
            />
          )}
          <Tooltip title={props.title ?? ""} placement='bottom'>
            <Box className={classes.title}>{props.title ?? "Sample Title"}</Box>
          </Tooltip>
        </Box>
        <Box className={classes.details}>
          <label className={classes.author}>{props.author ?? "John Doe"}</label>
          <label className={classes.date}>
            {props.date ?? "Wed., Jan 1, 2020  12:00pm"}
          </label>
        </Box>
      </Box>
      {props.error && (
        <Tooltip title={props.error ?? ""} placement='bottom'>
          <ErrorIcon className={classes.errorIcon} />
        </Tooltip>
      )}
      {getStatus()}
    </Box>
  );
}

export default withStyles(useStyles)(ChartCard);

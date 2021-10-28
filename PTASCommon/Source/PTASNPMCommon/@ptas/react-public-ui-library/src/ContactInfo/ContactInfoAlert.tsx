// ContactInfoAlert.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, Fragment } from "react";
import { createStyles, WithStyles, withStyles } from "@material-ui/core";
import { Theme } from "@material-ui/core/styles";
import { GenericWithStyles } from "@ptas/react-ui-library";
import { CustomPopover } from "./../CustomPopover";
import { Alert } from "./../Alert";
import clsx from "clsx";
import { v4 as uuid } from "uuid";
import { CustomTextButton } from "..";

export interface ListItem {
  value: number | string;
  label: string;
}

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  anchorEl: HTMLElement | null | undefined;
  onClose: () => void;
  onButtonClick: () => void;
  items: ListItem[];
  onItemClick: (item: ListItem) => void;
  buttonText?: string | React.ReactNode;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {},
    popover: {
      borderRadius: "10px"
    },
    alert: {
      padding: "40px 20px 21px 20px",
      borderRadius: "9px",
      fontSize: theme.ptas.typography.bodySmallBold.fontSize,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: "19px"
    },
    popupColor: {
      backgroundColor: theme.ptas.colors.theme.accent,
      color: theme.ptas.colors.theme.white
    },
    contactInfoLine: {
      display: "block",
      paddingLeft: theme.spacing(0.5),
      paddingRight: theme.spacing(0.5),
      marginBottom: theme.spacing(2),
      color: theme.ptas.colors.theme.white,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: "19px"
    },
    closePopupButton: {
      color: theme.ptas.colors.theme.white
    },
    backdrop: {
      backgroundColor: "transparent !important"
    },
    border: {
      background:
        "linear-gradient(90deg, rgba(255, 255, 255, 0) 0%, #FFFFFF 49.48%, rgba(255, 255, 255, 0) 100%)",
      height: 1,
      width: "100%",
      display: "block"
    },
    buttonManageContainer: {
      display: "flex",
      justifyContent: "center"
    },
    buttonManage: {
      width: "auto",
      height: 23,
      padding: "3px 22px",
      fontSize: 14,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: "17px",
      marginTop: theme.spacing(2)
    }
  });

/**
 * ContactInfoAlert
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ContactInfoAlert(props: Props): JSX.Element {
  const { classes, anchorEl, onClose, onButtonClick, items, buttonText } =
    props;

  const PopupContent = (
    <Fragment>
      {items.map((item) => (
        <CustomTextButton
          key={uuid()}
          ptasVariant='Text'
          classes={{ root: classes.contactInfoLine }}
          onClick={() => {
            props.onItemClick(item);
          }}
        >
          {item.label}
        </CustomTextButton>
      ))}
      <span className={classes.border}></span>
    </Fragment>
  );

  return (
    <CustomPopover
      classes={{
        root: `${classes.root} ${props.classes?.root}`,
        paper: classes.popover,
        tail: classes.popupColor,
        closeButton: classes.closePopupButton,
        backdropRoot: classes.backdrop
      }}
      anchorEl={anchorEl}
      onClose={(): void => {
        onClose();
      }}
      ptasVariant='info'
      showCloseButton
      tail
      tailPosition='end'
      anchorOrigin={{
        vertical: "bottom",
        horizontal: "right"
      }}
      transformOrigin={{
        vertical: "top",
        horizontal: "right"
      }}
    >
      <Alert
        classes={{
          root: clsx(classes.alert, classes.popupColor),
          buttons: classes.buttonManage,
          buttonContainer: classes.buttonManageContainer
        }}
        contentText={PopupContent}
        ptasVariant='info'
        okShowButton
        okButtonText={buttonText ?? "Manage"}
        okButtonClick={(): void => {
          onButtonClick();
        }}
      />
    </CustomPopover>
  );
}

export default withStyles(useStyles)(ContactInfoAlert) as FC<
  GenericWithStyles<Props>
>;
